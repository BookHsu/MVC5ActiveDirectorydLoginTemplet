using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc5OWINwithAD.Models.Override;

namespace Mvc5OWINwithAD.Controllers
{
    public class ErrorController : BaseController
    {
        // GET: Error
        public ActionResult General(string error)
        {
            TempData["ErrMsg"] = $"錯誤識別碼：{error}";
            return View("Error");
        }
    }
}