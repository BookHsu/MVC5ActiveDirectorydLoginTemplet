using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mvc5OWINwithAD
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            var code = (exception is HttpException) ? (exception as HttpException).GetHttpCode() : 500;
            var ActionType = Request.HttpMethod;
            var UserName = string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name) ? "未知的使用者" : HttpContext.Current.User.Identity.Name;
            string RequestUrl = Request.Url.ToString();
            //TODO 在此處紀錄log 
            //LibroLib.ShareMethod.PutLog("Global_Error", $"由{UserName}觸發{exception.GetType().FullName}");
            //LibroLib.ShareMethod.PutLog("Global_Error", $"HttpMethod:{ActionType}     Url:{RequestUrl}     StatusCode{code}");
            //LibroLib.ShareMethod.ExceptionLog("Global_Error", exception);
            Response.Clear();
            Response.Redirect("~/Home/Index");
            Server.ClearError();
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
