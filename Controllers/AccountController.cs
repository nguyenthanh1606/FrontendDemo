using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Service.Service;
using Frontend.Infrastructure.ExtensionMethod;
using Store.Data;
using AutoMapper;
using Frontend.Models;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Store.Service.Authorize;
using Microsoft.AspNet.Identity.Owin;
using Store.Data.Models;
using Frontend.Infrastructure.Attributes;
using Store.Service.ProductServices;
using Frontend.Infrastructure.Helpers;
using System.Net;
using Store.Service.Helper;
using Store.Service.SystemService;
using Resources;
using Store.Service.Helper.ExtensionMethod;
using System.Configuration;

namespace Frontend.Controllers
{
    [Authorize]
    public partial class AccountController : BaseFrontendController
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ISystemService _systemService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IGeneralService _generalService;
        private readonly ICustomerAddressService _customerAddressService;
        private readonly IOrderService _orderService;
        private readonly IProductVersionService _productVersionService;
        private readonly IShoppingCartDetailService _shoppingCartDetailService;
        private readonly IWishlistService _wishlistService;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            ISystemService systemService, IShoppingCartService shoppingCartService, IMapper mapper,
            IGeneralService generalService, ICustomerAddressService customerAddressService,
            IOrderService orderService, IProductVersionService productVersionService,
            IShoppingCartDetailService shoppingCartDetailService,
            IWishlistService wishlistService,
            IProductService productService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _systemService = systemService;
            _shoppingCartService = shoppingCartService;
            _generalService = generalService;
            _customerAddressService = customerAddressService;
            _orderService = orderService;
            _productVersionService = productVersionService;
            _shoppingCartDetailService = shoppingCartDetailService;
            _wishlistService = wishlistService;
            _productService = productService;
            _mapper = mapper;
        }

        

        #region Account function
        //
        // GET: /Account/Login
        [AllowAnonymous, ImportModelStateFromTempData]
        public virtual ActionResult Login(string returnUrl)
        {
            Session["EnableOwin"] = true;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            //ModelState.Remove("RegisterViewModel");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login", new { returnUrl = returnUrl });
            }

            bool emailConfirmRequired = Convert.ToBoolean(_systemService.GetAppPropertyValue(
                        AppPropertyString.MemberEmailIsChecked));
            if (emailConfirmRequired)
            {
                // Require the user to have a confirmed email before they can log on.
                var user = _userManager.Find(model.Email, model.Password);
                if (user != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user.Id))
                    {
                        ViewBag.Message = Resource.RegisterConfirmMailMsg;
                        ViewBag.Id = user.Id;
                        return View("EmailNotConfirmed");
                    }
                }
            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    UpdateCartIdForLoginCustomer(_signInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId<int>());
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("LoginViewModel", Resource.InvalidLoginErrorMsg);
                    return RedirectToAction("Login");
            }
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public virtual async Task<JsonResult> LoginAjax(LoginViewModel loginViewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToList();
                return Json(new { errors = errorList });
            }

            bool emailConfirmRequired = Convert.ToBoolean(_systemService.GetAppPropertyValue(
                        AppPropertyString.MemberEmailIsChecked));
            if (emailConfirmRequired)
            {
                // Require the user to have a confirmed email before they can log on.
                var user = _userManager.Find(loginViewModel.Email, loginViewModel.Password);
                if (user != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user.Id))
                    {
                        return Json(new { errors = new List<string>() { Resource.RegisterConfirmMailMsg } });
                    }
                }
            }


            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password,
                loginViewModel.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    UpdateCartIdForLoginCustomer(_signInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId<int>());
                    return Json(new { result = "Success" });
                case SignInStatus.LockedOut:
                    return Json(new { errors = new List<string>() { "Lockout" } });
                case SignInStatus.RequiresVerification:
                    return Json(new { result = "SendCode" });
                case SignInStatus.Failure:
                default:
                    return Json(new { errors = new List<string>() { Resource.InvalidLoginErrorMsg } });
            }
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResendConfirmMail(int id)
        {
            await SendEmailConfirmationTokenAsync(id, "Confirm your account-Resend");
            return Json(true);
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public virtual async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the AppMember has already logged in via username/password or external login
            if (!await _signInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var AppMember = await _userManager.FindByIdAsync(await _signInManager.GetVerifiedUserIdAsync());
            if (AppMember != null)
            {
                var code = await _userManager.GenerateTwoFactorTokenAsync(AppMember.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a AppMember enters incorrect codes for a specified amount of time then the AppMember account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public virtual ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual async Task<ActionResult> Register(AccountViewModel model)
        {
            ModelState.Remove("LoginViewModel");
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(model.RegisterViewModel);
                user.UserName = user.Email;
                var result = await _userManager.CreateAsync(user, model.RegisterViewModel.Password);
                if (result.Succeeded)
                {
                    bool emailConfirmRequired = Convert.ToBoolean(_systemService.GetAppPropertyValue(
                        AppPropertyString.MemberEmailIsChecked));
                    if (emailConfirmRequired)
                    {
                        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");

                        ViewBag.Message = Resource.RegisterConfirmMailMsg;
                        ViewBag.Id = user.Id;
                        return View("EmailNotConfirmed");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        UpdateCartIdForLoginCustomer(user.Id);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    AddErrors(result, "RegisterViewModel");
                    return RedirectToAction("Login");
                }

            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("RegisterViewModel", Resource.UnknowErrorMessage);
            return RedirectToAction("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual async Task<ActionResult> RegisterAjax(AccountViewModel model)
        {
            ModelState.Remove("LoginViewModel");
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<ApplicationUser>(model.RegisterViewModel);
                var result = await _userManager.CreateAsync(user, model.RegisterViewModel.Password);
                if (result.Succeeded)
                {
                    bool emailConfirmRequired = Convert.ToBoolean(_systemService.GetAppPropertyValue(
                        AppPropertyString.MemberEmailIsChecked));
                    if (emailConfirmRequired)
                    {
                        string callbackUrl = await SendEmailConfirmationTokenAsync(user.Id, "Confirm your account");

                        ViewBag.Message = Resource.RegisterConfirmMailMsg;
                        ViewBag.Id = user.Id;
                        return View("EmailNotConfirmed");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        UpdateCartIdForLoginCustomer(user.Id);
                        return Json(new { result = "Success" });
                    }
                }
                else
                {
                    AddErrors(result, "RegisterViewModel");
                    return RedirectToAction("Login");
                }

            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("RegisterViewModel", Resource.UnknowErrorMessage);
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public virtual async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(Int32.Parse(userId), code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public virtual ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var AppMember = await _userManager.FindByNameAsync(model.Email);
                if (AppMember == null || !(await _userManager.IsEmailConfirmedAsync(AppMember.Id)))
                {
                    // Don't reveal that the AppMember does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string code = await _userManager.GeneratePasswordResetTokenAsync(AppMember.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = AppMember.Id, code = code }, protocol: Request.Url.Scheme);
                await _userManager.SendEmailAsync(AppMember.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public virtual ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var AppMember = await _userManager.FindByNameAsync(model.Email);
            if (AppMember == null)
            {
                // Don't reveal that the AppMember does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await _userManager.ResetPasswordAsync(AppMember.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public virtual ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [ImportModelStateFromTempData]
        public virtual ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", Resource.PasswordNotMatch);
                    return RedirectToAction("ChangePassword");
                }

                var result = _userManager.ChangePassword(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Info");
                }
                else
                {
                    AddErrors(result);
                    return RedirectToAction("ChangePassword");
                }
            }
            ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
            return RedirectToAction("ChangePassword");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ExternalLogin(string provider, string returnUrl)
        {
            //ControllerContext.HttpContext.Session.RemoveAll();
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public virtual async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await _signInManager.GetVerifiedUserIdAsync();
            if (userId == 0)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await _signInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public virtual async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null || loginInfo.Email == null)
            {
                return RedirectToAction("Login");
            }
            //check if that user with email already has account, if has add to accountLogin then signin
            ApplicationUser user = await _userManager.FindByNameAsync(loginInfo.Email);
            if (user != null)
            {
                var addLoginResult = await _userManager.AddLoginAsync(user.Id, loginInfo.Login);
                if (addLoginResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    UpdateCartIdForLoginCustomer(user.Id);
                    return RedirectToLocal(returnUrl);
                }
            }

            // Sign in the AppMember with this external login provider if the AppMember already has a login
            var result = await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    UpdateCartIdForLoginCustomer(_signInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId<int>());
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the AppMember does not have an account, create one and login
                    //check if email alreay existed as username
                    string mailUsername = loginInfo.Email.Split('@')[0];
                    string userName = null;
                    do
                    {
                        var member = _userManager.FindByName(mailUsername);

                        //if not existed, username = email
                        if (member == null)
                        {
                            userName = mailUsername;
                        }
                        else
                        {
                            mailUsername = mailUsername + "_1";
                        }
                    }
                    while (string.IsNullOrEmpty(userName));


                    var AppMember = new ApplicationUser
                    {
                        UserName = userName,
                        Email = loginInfo.Email,
                        EmailConfirmed = true,
                    };
                    //Get birthday
                    var birthdayClaim = loginInfo.ExternalIdentity.FindFirst(ClaimTypes.DateOfBirth);
                    if (birthdayClaim != null)
                    {
                        string birthdayStr = birthdayClaim.Value.ToString();
                        //check not contain year in google -> set deafault year 1980
                        if (birthdayStr.StartsWith("0000"))
                        {
                            birthdayStr = birthdayStr.Replace("0000", "1980");
                        }
                        DateTime birthday;
                        if (!string.IsNullOrEmpty(birthdayStr))
                        {
                            if (DateTime.TryParse(birthdayStr, out birthday))
                            {
                                //year smaller than sql allow
                                if (birthday.Year < 1753)
                                {
                                    birthday = birthday.AddYears(1754 - birthday.Year);
                                }
                                AppMember.Birthday = birthday;
                            }
                        }
                    }
                    else
                    {
                        DateTime birthday = new DateTime(2000, 1, 1);
                        AppMember.Birthday = birthday;
                    }

                    //Get name
                    var nameClaim = loginInfo.ExternalIdentity.FindFirst(ClaimTypes.Name);
                    if (nameClaim != null)
                    {
                        string name = nameClaim.Value.ToString();
                        if (!string.IsNullOrEmpty(name))
                        {
                            AppMember.FullName = name;
                        }
                        else
                        {
                            AppMember.FullName = mailUsername;
                        }
                    }

                    //Get gender
                    var genderClaim = loginInfo.ExternalIdentity.FindFirst(ClaimTypes.Gender);
                    if (genderClaim != null)
                    {
                        string gender = loginInfo.ExternalIdentity.FindFirst(ClaimTypes.Gender).Value.ToString();
                        if (!string.IsNullOrEmpty(gender))
                        {
                            if (gender == "male")
                            {
                                AppMember.Gender = 0;
                            }
                            if (gender == "female")
                            {
                                AppMember.Gender = 1;
                            }
                        }
                    }
                    //try create and save info
                    var createResult = await _userManager.CreateAsync(AppMember);
                    if (createResult.Succeeded)
                    {
                        createResult = await _userManager.AddLoginAsync(AppMember.Id, loginInfo.Login);
                        if (createResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(AppMember, isPersistent: false, rememberBrowser: false);
                            UpdateCartIdForLoginCustomer(AppMember.Id);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    return RedirectToLocal(returnUrl);
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the AppMember from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var AppMember = new ApplicationUser
                {
                    UserName = info.Email,
                    Email = info.Email,
                    Gender = model.Gender,
                    Birthday = DateTime.Parse(model.Birthday),
                    PhoneNumber = model.Phone,
                    FullName = model.Name
                };
                var result = await _userManager.CreateAsync(AppMember);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(AppMember.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(AppMember, isPersistent: false, rememberBrowser: false);
                        UpdateCartIdForLoginCustomer(AppMember.Id);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        public virtual ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public virtual ActionResult ExternalLoginFailure()
        {
            return View();
        }
        #endregion

        #region Shop related function
        public virtual ActionResult Info()
        {
            AccountInfo info = new AccountInfo();
            info.CustomerInfo = _mapper.Map<AccountBasicInfo>(_userManager.FindByName(User.Identity.GetUserName()));
            info.CustomerAddress = _mapper.Map<IEnumerable<CustomerAddressOverviewViewModel>>(
                        _customerAddressService.GetListAddress(User.Identity.GetUserId<int>())).ToList();
            return View(info);
        }
        public virtual ActionResult Address()
        {
            AccountInfo info = new AccountInfo();
            info.CustomerInfo = _mapper.Map<AccountBasicInfo>(_userManager.FindByName(User.Identity.GetUserName()));
            info.CustomerAddress = _mapper.Map<IEnumerable<CustomerAddressOverviewViewModel>>(
                        _customerAddressService.GetListAddress(User.Identity.GetUserId<int>())).ToList();
            return View(info);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public virtual ActionResult EditBasicInfo(AccountInfo model)
        {
            ModelState.Remove("CustomerAddress");
            ModelState.Remove("NewAddress");
            if (ModelState.IsValid)
            {
                ApplicationUser user = _userManager.FindByName(User.Identity.Name);
                user.Birthday = DateTime.Parse(model.CustomerInfo.Birthday);
                user.FullName = model.CustomerInfo.Fullname;
                user.PhoneNumber = model.CustomerInfo.Phone;
                user.Gender = model.CustomerInfo.Gender;
                var updateResult = _userManager.Update(user);
                if (updateResult.Succeeded)
                {
                    return Json(new { result = "ok" });
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return Json(new { errorMsg = updateResult.Errors });
                }
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { errorMsg = Resource.UnknowErrorMessage });
        }

        [ImportModelStateFromTempData]
        public virtual ActionResult AddAddress()
        {
            ViewBag.ListCity = new SelectList(_generalService.GetAllCity().Where(t => t.Value1.Trim() == "1"), "Id", "Name");
            ViewBag.ListDistrict = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ListWard = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.ListAddressType = new SelectList(_generalService.GetAllAddressType(), "AddressTypeID", "AddressTypeName");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult AddAddress(CustomerAddressViewModel model, bool isAjax = false)
        {
            if (ModelState.IsValid)
            {
                CustomerAddress address = _mapper.Map<CustomerAddress>(model);
                address.CustomerID = User.Identity.GetUserId<int>();
                var newAddressId = _customerAddressService.Insert(address);
                if (newAddressId.HasValue)
                {
                    if (isAjax)
                    {
                        var returnAddress = _mapper.Map<CustomerAddressOverviewViewModel>(
                            _customerAddressService.GetAddressWithNameById(newAddressId.Value));
                        return Json(returnAddress);
                    }
                    return RedirectToAction("Info");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
                }
            }

            if (isAjax)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { errorMsg = Resource.UnknowErrorMessage });
            }
            return RedirectToAction("AddAddress");
        }

        [ImportModelStateFromTempData]
        public virtual ActionResult EditAddress(int id = 0) 
        {
            CustomerAddress address = _customerAddressService.Get(id);
            if (address == null || address.CustomerID != User.Identity.GetUserId<int>())
            {
                return HttpNotFound();
            }

            CustomerAddressViewModel model = _mapper.Map<CustomerAddressViewModel>(address);
            ViewBag.ListCity = new SelectList(_generalService.GetAllCity().Where(t => t.Value1.Trim() == "1"), "Id", "Name");
            ViewBag.ListDistrict = new SelectList(_generalService.GetDistrictForCity(address.Province), "Id", "Name");
            ViewBag.ListWard = new SelectList(_generalService.GetWardForDistrict(address.District), "Id", "Name");
            ViewBag.ListAddressType = new SelectList(_generalService.GetAllAddressType(), "AddressTypeID", "AddressTypeName");

            return View(model);
        }

        public virtual ActionResult EditAddressCheckout(int? id = null)
        {
            //if no add new
            CustomerAddressViewModel model;
            ViewBag.IsEdit = true;
            if (!id.HasValue)
            {
                model = new CustomerAddressViewModel();
                ViewBag.ListCity = new SelectList(_generalService.GetAllCity().Where(t => t.Value1.Trim() == "1"), "Id", "Name");
                ViewBag.ListDistrict = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ListWard = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.ListAddressType = new SelectList(_generalService.GetAllAddressType(), "AddressTypeID", "AddressTypeName");
                ViewBag.IsEdit = false;
                return PartialView(model);
            }


            CustomerAddress address = _customerAddressService.Get(id.Value);
            if (address == null || address.CustomerID != User.Identity.GetUserId<int>())
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { errorMsg = Resource.UnknowErrorMessage });
            }

            model = _mapper.Map<CustomerAddressViewModel>(address);
            ViewBag.ListCity = new SelectList(_generalService.GetAllCity().Where(t => t.Value1.Trim() == "1"), "Id", "Name");
            ViewBag.ListDistrict = new SelectList(_generalService.GetDistrictForCity(address.Province), "Id", "Name");
            ViewBag.ListWard = new SelectList(_generalService.GetWardForDistrict(address.District), "Id", "Name");
            ViewBag.ListAddressType = new SelectList(_generalService.GetAllAddressType(), "AddressTypeID", "AddressTypeName");

            return PartialView(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult EditAddress(CustomerAddressViewModel model, bool isAjax = false)
        {
            if (ModelState.IsValid)
            {
                CustomerAddress address = _mapper.Map<CustomerAddress>(model);
                address.CustomerID = User.Identity.GetUserId<int>();
                if (_customerAddressService.Update(address))
                {
                    if (isAjax)
                    {
                        return Json(new { listAddress = _customerAddressService.GetListAddress(User.Identity.GetUserId<int>()) });
                    }
                    return RedirectToAction("Address");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
                }
            }
            if (isAjax)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { errorMsg = Resource.UnknowErrorMessage });
            }
            return RedirectToAction("EditAddress");
        }

        public virtual ActionResult MyOrders(string orderID = null)
        {
            IEnumerable<CustomerOrderViewModel> model = _mapper.Map<IEnumerable<CustomerOrderViewModel>>(
              _orderService.GetOrderForCustomer(User.Identity.GetUserId<int>()));

            if (!string.IsNullOrEmpty(orderID))
            {
                model = model.Where(c => c.OrderID.ToString().ToLower() == orderID.ToLower());
            }

            return View(model);
        }

        [ImportModelStateFromTempData]
        public virtual ActionResult OrderDetails(int id = 0)
        {
            Order order = _orderService.Get(id, true);
            if (order == null || order.CustomerID != User.Identity.GetUserId<int>())
            {
                return HttpNotFound();
            }
            CustomerOrderViewModel model = _mapper.Map<CustomerOrderViewModel>(order);
            foreach (var item in model.OrderCart)
            {
                item.ListProperties = _productVersionService.GetProductPropertiesValue(item.ProductVersionID);
            }

            //list cancel reason
            var cancelReasons = from CancelOrderReason stt in Enum.GetValues(typeof(CancelOrderReason))
                                select new
                                {
                                    Id = (int)stt,
                                    Name = stt.GetDescription()
                                };
            ViewBag.CancelOrderReasons = new SelectList(cancelReasons, "Id", "Name");

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
        public virtual ActionResult CancelOrder(int id, CancelOrderReason cancelReason, string note)
        {
            Order order = _orderService.Get(id);
            if (order != null && order.CustomerID == User.Identity.GetUserId<int>())
            {
                if (_orderService.UpdateStatus(id, OrderStatus.Cancel, (int)cancelReason, note))
                {
                    return RedirectToAction("MyOrders");
                }
            }

            ModelState.AddModelError(string.Empty, Resource.UnknowErrorMessage);
            return RedirectToAction("OrderDetails", new { id = id });
        }

        [AllowAnonymous]
        public virtual PartialViewResult LoginPartial()
        {
            ViewBag.ReturnUrl = Request.Url.PathAndQuery;
            return PartialView(new AccountViewModel());
        }

        [AllowAnonymous, HttpPost]
        public virtual ActionResult CheckOrder(SearchOrderViewModel model)
        {
            List<CustomerOrderViewModel> result = new List<CustomerOrderViewModel>();
            var account = _userManager.FindByName(model.Email);
            if (account == null)
                return View(result);

            result = _mapper.Map<IEnumerable<CustomerOrderViewModel>>(
               _orderService.GetOrderForCustomer(account.Id).Where(c => c.OrderID.ToString() == model.OrderID)).ToList();

            var test = _orderService.GetOrderForCustomer(account.Id);
            return View(result);
        }


        public virtual ActionResult WishList()
        {
            var result = _mapper.Map<IEnumerable<WishlistViewModel>>(_wishlistService.GetListWishlist(ShoppingCartHelper.GetCartId()));

            foreach (var favorite in result)
            {
                favorite.ProductDetails = _mapper.Map<ProductBoxViewModel>(_productService.Get(favorite.ProductId));
            }
            return View(result);
        }

        #endregion

        #region Helper
        /// <summary>
        /// Add all items from tempCart to customerCart and remove tempCookie
        /// </summary>
        [NonAction]
        public void UpdateCartIdForLoginCustomer(int userId)
        {
            var tempCartId = this.Request.Cookies[ShoppingCartHelper.CartKey];
            if (tempCartId != null)
            {
                _shoppingCartService.UpdateCartId(tempCartId.Value, userId.ToString());
                //delete tempCart cookie
                var tempCookie = new HttpCookie(ShoppingCartHelper.CartKey);
                tempCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(tempCookie);
            }
        }

        [HttpPost]
        public virtual JsonResult DoesUserNameExist(string UserName)
        {

            var account = _userManager.FindByName(UserName);
            return Json(account == null);
        }

        private async Task<string> SendEmailConfirmationTokenAsync(int userID, string subject)
        {
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userID);
            var callbackUrl = Url.Action("ConfirmEmail", "Account",
               new { userId = userID, code = code }, protocol: Request.Url.Scheme);
            await _userManager.SendEmailAsync(userID, subject,
               "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            return callbackUrl;
        }

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result, string modelName = "")
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(modelName, error);
            }
        }



        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

       
    }
}