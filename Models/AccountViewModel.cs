using Resources;
using Resources;
using Store.Service.Helper.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class AccountViewModel
    {
        public AccountViewModel()
        {
            LoginViewModel = new LoginViewModel();
            RegisterViewModel = new RegisterViewModel();
        }
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "Email", ResourceType = typeof(Resource))]
        [MaxLength(256, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Email { get; set; }

        [DisplayResource(Name = "Fullname", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(256, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(15, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [DisplayResource(Name = "Gender", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int? Gender { get; set; }

        [DisplayResource(Name = "Birthday", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Birthday { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class AccountInfo
    {
        public AccountInfo()
        {
            CustomerAddress = new List<CustomerAddressOverviewViewModel>();
        }

        public AccountBasicInfo CustomerInfo { get; set; }

        public List<CustomerAddressOverviewViewModel> CustomerAddress { get; set; }
    }

    public class AccountBasicInfo
    {
        public string Email { get; set; }

        [DisplayResource(Name = "Fullname", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(256, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Fullname { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayResource(Name = "Phone", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(15, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        [DisplayResource(Name = "Gender", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int? Gender { get; set; }

        [DisplayResource(Name = "Birthday", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Birthday { get; set; }
    }

    public class CustomerAddressOverviewViewModel
    {
        public int Id { get; set; }

        [DisplayResource(Name = "Fullname", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string CustomerName { get; set; }

        [DisplayResource(Name = "Phone", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }
        public string Address { get; set; }
        public int ShippingFee { get; set; }
    }

    public class CustomerAddressViewModel
    {
        public int? Id { get; set; }

        [DisplayResource(Name = "Fullname", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(64, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string CustomerName { get; set; }

        [DisplayResource(Name = "Phone", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(15, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Phone { get; set; }

        public string Nation { get; set; }

        [DisplayResource(Name = "City", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int? Province { get; set; }

        [DisplayResource(Name = "District", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int? District { get; set; }

        [DisplayResource(Name = "Ward", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int? Ward { get; set; }

        [DisplayResource(Name = "Address", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [MaxLength(128, ErrorMessage = null, ErrorMessageResourceName = "MaxLengthError", ErrorMessageResourceType = typeof(Resource))]
        public string Address { get; set; }

        [DisplayResource(Name = "AddressType", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public int AddressType { get; set; }

        [DisplayResource(Name = "DefaultAddress", ResourceType = typeof(Resource))]
        public bool IsDefaultAddress { get; set; }

        public int ShippingFee { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [DisplayResource(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [DisplayResource(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [DisplayResource(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [DisplayResource(Name = "RememberMe", ResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public bool RememberMe { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [StringLength(100, ErrorMessageResourceName = "StringLengthErrorMessage", ErrorMessageResourceType = typeof(Resource), MinimumLength = 6)]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "OldPassword", ResourceType = typeof(Resource))]
        public string OldPassword { get; set; }

        [StringLength(100, ErrorMessageResourceName = "StringLengthErrorMessage", ErrorMessageResourceType = typeof(Resource), MinimumLength = 6)]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "NewPassword", ResourceType = typeof(Resource))]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }


    }

    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceName = "EmailNotValid", ErrorMessageResourceType = typeof(Resource))]
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "Email", ResourceType = typeof(Resource))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]

        [StringLength(100, ErrorMessageResourceName = "StringLengthErrorMessage", ErrorMessageResourceType = typeof(Resource), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayResource(Name = "Password", ResourceType = typeof(Resource))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayResource(Name = "ConfirmPassword", ResourceType = typeof(Resource))]
        [Compare("Password", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(Resource))]
        public string ConfirmPassword { get; set; }

        [DisplayResource(Name = "Fullname", ResourceType = typeof(Resource))]
        public string Fullname { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DisplayResource(Name = "Gender", ResourceType = typeof(Resource))]
        public int? Gender { get; set; }

        [DisplayResource(Name = "Birthday", ResourceType = typeof(Resource))]
        public string Birthday { get; set; }

        public bool AcceptToS { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayResource(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayResource(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [DisplayResource(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [DisplayResource(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [StringLength(100, ErrorMessageResourceName = "StringLengthErrorMessage", ErrorMessageResourceType = typeof(Resource), MinimumLength = 5)]
        [DisplayResource(Name = "Username", ResourceType = typeof(Resource))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public string ProviderKey { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        [DisplayResource(Name = "Department", ResourceType = typeof(Resource))]
        public string DepartmentID { get; set; }

        [Required(ErrorMessageResourceName = "RequiredError", ErrorMessageResourceType = typeof(Resource))]
        public bool RememberMe { get; set; }
    }
}