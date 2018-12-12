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
            string errorMsg = "Global Error Key:{0} 發生例外網址:{1}  HttpMethod:{2} 使用者:{3}";
            Exception exception = Server.GetLastError();
            HttpContext httpContext = HttpContext.Current;
            RouteData routeData = new RouteData();
            HttpException httpException = exception as HttpException;
            string GuidKey = Math.Abs(exception.GetHashCode()).ToString();
            string httpErrorCode = string.Empty;
            var ActionType = Request.HttpMethod;
            var UserName = string.IsNullOrWhiteSpace(httpContext.User.Identity.Name) ? "NaN" : httpContext.User.Identity.Name;
            if (httpException != null)
            {
                httpErrorCode = httpException.GetHttpCode().ToString();
            }
            errorMsg = string.Format(errorMsg, GuidKey, Request.Url.ToString(), ActionType, UserName);
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("error", GuidKey);
            switch (httpErrorCode)
            {
                case "404":
                    routeData.Values.Add("action", "General");
                    break;
                case "500":
                    routeData.Values.Add("action", "General");
                    break;
                default:
                    routeData.Values.Add("action", "General");
                    break;
            }

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
