using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Mvc5OWINwithAD.Models.Override
{
    /// <summary>
    /// Controller Base
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 利用attr設定標題
        /// </summary>
        public string PageTitle { set { ViewData["PageTitle"] = value; } }
        /// <summary>
        /// 取得CliaimIdentity
        /// </summary>
        public ClaimsIdentity CliaimIdentity { get { return User.Identity as ClaimsIdentity; } }
        public BaseController() : base()
        {

        }
        /// <summary>
        /// 利用ATTR設定標題
        /// </summary>
        /// <returns></returns>
        protected override IActionInvoker CreateActionInvoker()
        {
            return new CustomerCreateActionInvoker();
        }
        

        /// <summary>
        /// 複寫JSON改用JsonNet
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {

            if (behavior == JsonRequestBehavior.DenyGet && string.Equals(this.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return new JsonResult();

            return new JsonNetResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };

        }

    }
}