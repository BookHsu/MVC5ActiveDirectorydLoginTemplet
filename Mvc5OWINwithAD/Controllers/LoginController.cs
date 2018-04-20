using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Mvc5OWINwithAD.Models;
using Mvc5OWINwithAD.Models.Override;

namespace Mvc5OWINwithAD.Controllers
{
    public class LoginController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous,HttpPost,ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model,string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);

            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            var authservice = new AdAuthenticationService(authenticationManager);
            var authenticationResult = authservice.SignIn(model.UserName, model.Password);
            if (authenticationResult.IsSuccess)
            {
                return RedirecToLocal(returnUrl);
            }
            ModelState.AddModelError("", authenticationResult.ErrorMessage);
            return View(model);
        }
        private ActionResult RedirecToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            IAuthenticationManager manager = HttpContext.GetOwinContext().Authentication;
            manager.SignOut(MyAuthentication.ApplicationCookie);
          return  RedirectToAction("Index");
        }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}