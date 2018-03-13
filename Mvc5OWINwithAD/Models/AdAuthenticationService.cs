using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.Owin.Security;

namespace Mvc5OWINwithAD.Models
{
    public class AdAuthenticationService
    {
        public class AuthenticationResult
        {
            public AuthenticationResult(string errorMessage=null)
            {
                ErrorMessage = errorMessage;
            }
            public string ErrorMessage { get;private set; }
            public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);
        }
        private readonly IAuthenticationManager authenticationManager;

        public AdAuthenticationService(IAuthenticationManager authentication)
        {
            authenticationManager = authentication;
        }
        /// <summary>
        /// 執行AD登入
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthenticationResult SignIn(string username,string password)
        {
            ContextType authType = ContextType.Domain;
            //TODO 修改AD網域名稱
            PrincipalContext principalContext = new PrincipalContext(authType,"anser-u2.com");
            bool isAuth = false;
            UserPrincipal userPrincipal = null;
            try
            {
                isAuth = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);
                if (isAuth) userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
            }
            catch (Exception)
            {

                isAuth = false;
                userPrincipal = null;
                /*可在此處紀錄登入失敗者*/
            }
            if (!isAuth || userPrincipal == null)
            {
                return new AuthenticationResult("帳號或密碼錯誤");
            }

            if (userPrincipal.IsAccountLockedOut())
            {
                return new AuthenticationResult("帳號被鎖定，請洽資訊部人員");
            }

            if (userPrincipal.Enabled.HasValue&&userPrincipal.Enabled.Value==false)
            {
                return new AuthenticationResult("帳號已被停用");
            }

            var identity = CreateIdentity(userPrincipal);
            authenticationManager.SignOut(MyAuthentication.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
            return new AuthenticationResult();
        }
        private ClaimsIdentity CreateIdentity(UserPrincipal userPrincipal)
        {

            var identity = new ClaimsIdentity(MyAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Active Directory"));
            //TODO name為User.Identity.Name
            identity.AddClaim(new Claim(ClaimTypes.Name, culture.TextInfo.ToTitleCase(userPrincipal.DisplayName)));
            /*可加入所需使用的Claims*/
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, culture.TextInfo.ToTitleCase(userPrincipal.SamAccountName),null,"EnName"));
            if (!string.IsNullOrWhiteSpace(userPrincipal.EmailAddress))
            {
                /*其他地方如需使用此處資料--
                 *             var identity = User.Identity as ClaimsIdentity;
                 *             var claims= identity.Claims.Where(d => d.Issuer == "Email").FirstOrDefault();
                 *             string email= claims.Value;
                 */
                identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.EmailAddress,null,"Email"));
            }
            /*可在此處加入claims*/
            return identity;
        }
    }
}