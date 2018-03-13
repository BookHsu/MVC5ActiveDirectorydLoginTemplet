using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Mvc5OWINwithAD
{
    public static class MyAuthentication
    {
        public const string ApplicationCookie = "MyProjectAuthenticationType";
    }
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = MyAuthentication.ApplicationCookie,
                LoginPath = new PathString("/Login"),/*導入controller位置*/
                Provider = new CookieAuthenticationProvider(),
                CookieName = "MyCookieName",/*想要的CookieName*/
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromHours(12)/*Cookie時間可自行設定*/
            });
        }
    }
}