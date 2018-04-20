using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Mvc5OWINwithAD.Models.Override
{
    /// <summary>
    /// 改用JsonNet
    /// </summary>
    public class JsonNetResult: JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentException("Context is Null");

            var response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrWhiteSpace(ContentType) ? "application/json" : ContentType;
            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;
            if (Data != null)
            {
                response.Write(JsonConvert.SerializeObject(Data));
            }
        }
    }
}