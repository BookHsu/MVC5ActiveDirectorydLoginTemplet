using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Mvc5OWINwithAD.Models.Override;

namespace Mvc5OWINwithAD.Controllers
{
    public class HomeController : BaseController
    {
        [Filters.PageTitle("Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Filters.PageTitle("About")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Filters.PageTitle("Contact")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}