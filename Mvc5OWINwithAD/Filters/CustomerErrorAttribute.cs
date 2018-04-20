using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc5OWINwithAD.Filters
{
    /// <summary>
    /// 自定義錯誤處理
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CustomerErrorAttribute: HandleErrorAttribute
    {
        private const string DefaultView = "Error";
        public Exception ex { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string ActionType { get; set; }
        public string UserName { get; set; }
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.Exception == null)
            {
                throw new ArgumentNullException("Exception");
            }
            ex = filterContext.Exception;
            ActionName = filterContext.RouteData.Values["action"].ToString();
            ControllerName = filterContext.RouteData.Values["controller"].ToString();
            ActionType = filterContext.HttpContext.Request.HttpMethod;
            UserName =string.IsNullOrWhiteSpace(filterContext.HttpContext.User.Identity.Name) ? "未知的使用者" : filterContext.HttpContext.User.Identity.Name;
            string GuidKey = ex.GetHashCode().ToString();
            var fucntionName = $"{ControllerName}_{ActionName}_{ActionType}";
            //TODO 可加入LOG記錄在此處
            //LibroLib.ShareMethod.PutLog(fucntionName, $"由{UserName}觸發{ExceptionType.FullName}:識別碼：{GuidKey}");
            //LibroLib.ShareMethod.ExceptionLog(fucntionName, ex);
            var viewResult = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary()
            };
            viewResult.TempData["ErrMsg"] = $"錯誤識別碼：{GuidKey}";
            filterContext.Result = viewResult;
            filterContext.ExceptionHandled = true;
        }

    }


}