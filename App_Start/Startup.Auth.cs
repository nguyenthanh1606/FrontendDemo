using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Frontend.Models;
using System.Security.Claims;
using Microsoft.Owin.Security.Facebook;
using System.Net.Http;
using System.Configuration;

namespace Frontend
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: "525423534899075",
               appSecret: "f6ba7189ada431384ab444963094c0ad");

            //string FacebookAppId = ConfigurationManager.AppSettings["FacebookAppId"];
            //string FacebookAppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"];
            //if(!string.IsNullOrEmpty(FacebookAppId) && !string.IsNullOrEmpty(FacebookAppSecret))
            //{
            //    var facebookOptions = new FacebookAuthenticationOptions
            //    {
            //        AppId = FacebookAppId,
            //        AppSecret = FacebookAppSecret,
            //        Scope = { "email" },
            //        BackchannelHttpHandler = new FacebookBackChannelHandler(),
            //        UserInformationEndpoint = "https://graph.facebook.com/v2.8/me?fields=id,name,gender,birthday,email",
            //        Provider = new FacebookAuthenticationProvider()
            //        {
            //            OnAuthenticated = async context =>
            //            {
            //                context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
            //                if (context.User["birthday"] != null)
            //                {
            //                    context.Identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, context.User["birthday"].ToString()));
            //                }
            //                if (context.User["gender"] != null)
            //                {
            //                    context.Identity.AddClaim(new Claim(ClaimTypes.Gender, context.User["gender"].ToString()));
            //                }
            //                if (context.User["name"] != null)
            //                {
            //                    context.Identity.AddClaim(new Claim(ClaimTypes.Name, context.User["name"].ToString()));
            //                }
            //            }
            //        }
            //    };
            //    facebookOptions.Scope.Add("email");
            //    app.UseFacebookAuthentication(facebookOptions);
            //}

            string GoogleClientId = ConfigurationManager.AppSettings["GoogleClientId"];
            string GoogleClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"];
            if(!string.IsNullOrEmpty(GoogleClientId) && !string.IsNullOrEmpty(GoogleClientSecret))
            {
                var googlePlusOptions = new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = GoogleClientId,
                    ClientSecret = GoogleClientSecret,
                    SignInAsAuthenticationType = "ExternalCookie",
                    Provider = new GoogleOAuth2AuthenticationProvider()
                    {
                        OnAuthenticated = async context =>
                        {
                            context.Identity.AddClaim(new System.Security.Claims.Claim("GoogleAccessToken", context.AccessToken));
                            if (context.User["birthday"] != null)
                            {
                                context.Identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, context.User["birthday"].ToString()));
                            }
                            if (context.User["gender"] != null)
                            {
                                context.Identity.AddClaim(new Claim(ClaimTypes.Gender, context.User["gender"].ToString()));
                            }
                            if (context.User["displayname"] != null)
                            {
                                context.Identity.AddClaim(new Claim(ClaimTypes.Name, context.User["displayName"].ToString()));
                            }
                            if (context.User["image"] != null)
                            {
                                context.Identity.AddClaim(new Claim("ProfileImage", context.User["image"].ToString()));
                            }
                        }
                    }
                };
                app.UseGoogleAuthentication(googlePlusOptions);
            }
            
        }
    }

    //public class FacebookBackChannelHandler : HttpClientHandler
    //{
    //    protected override async System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    //    {
    //        // Replace the RequestUri so it's not malformed
    //        if (!request.RequestUri.AbsolutePath.Contains("/oauth"))
    //        {
    //            request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
    //        }

    //        return await base.SendAsync(request, cancellationToken);
    //    }
    //}
}