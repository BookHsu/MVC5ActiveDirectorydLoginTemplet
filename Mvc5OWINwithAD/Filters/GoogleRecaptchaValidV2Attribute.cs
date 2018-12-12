using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Mvc5OWINwithAD.Filters
{
    /// <summary>
    /// Google reCaptcha v2 驗證包裝
    /// 填入SecretKey時會驗證是否有通過google recaptcha驗證
    /// 填入SiteKey時會將SiteKey寫入 ViewData["gSiteKey"]中
    /// </summary>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class GoogleRecaptchaValidV2Attribute : ActionFilterAttribute
    {
        private const string formKey = "g-recaptcha-response";
        private const string googleApiUrl = "https://www.google.com/recaptcha/api/siteverify";

        /// <summary>至少必須填入SiteKey</summary>
        /// <param name="siteKey">會將SiteKey寫入 ViewData["gSiteKey"]中</param>
        public GoogleRecaptchaValidV2Attribute(string siteKey)
        {
            SiteKey = siteKey;
        }

        private string _secretKey = string.Empty;

        /// <summary>Google SecretKey
        /// 可選填 可傳入值或傳入Appsetting Key
        /// 填入此值會驗證google recaptcha
        /// </summary>
        public string SecretKey
        {
            get
            {
                return _secretKey;
            }
            set
            {
                try
                {
                    _secretKey = System.Configuration.ConfigurationManager.AppSettings[value];
                }
                catch (KeyNotFoundException ex)
                {
                    _secretKey = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private string _SiteKey = string.Empty;

        /// <summary>Google SiteKey 
        /// 可傳入值或傳入Appsetting Key
        ///  填入此值將寫入ViewData["gSiteKey"]
        /// </summary>
        public string SiteKey
        {
            get
            {
                return _SiteKey;
            }
            set
            {
                try
                {
                    _SiteKey = System.Configuration.ConfigurationManager.AppSettings[value];
                }
                catch (KeyNotFoundException ex)
                {
                    _SiteKey = value;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 驗證失敗錯誤訊息
        /// </summary>
        public string ErrorMessage { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(SecretKey))
            {
                var request = filterContext.HttpContext.Request;
                if (request == null || request.Form == null || !request.Form.Keys.Cast<string>().Contains(formKey))
                {
                    AddModelError(filterContext, "驗證失敗");
                }
                else
                {
                    string strToken = request.Form[formKey];
                    if (!ValidateGoogleReCaptcha(strToken))
                    {
                        AddModelError(filterContext, "驗證失敗");
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!string.IsNullOrWhiteSpace(SiteKey)) filterContext.Controller.ViewData["gSiteKey"] = SiteKey;
            base.OnActionExecuted(filterContext);
        }

        private void AddModelError(ActionExecutingContext filterContext, string strErrorMsg)
        {
            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                strErrorMsg = ErrorMessage;
            }
            filterContext.Controller.ViewData.ModelState.AddModelError(formKey, strErrorMsg);
        }

        private bool ValidateGoogleReCaptcha(string strToken)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string,string>(ApiKeyPair.secret.ToString(),SecretKey),
                    new KeyValuePair<string, string>(ApiKeyPair.response.ToString(),strToken),
                    new KeyValuePair<string, string>(ApiKeyPair.remoteip.ToString(),HttpContext.Current.Request.UserHostAddress)
                });
                var result = client.PostAsync(googleApiUrl, content).Result;
                if (result.IsSuccessStatusCode)
                {
                    var strResponse = result.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<GoogleResponse>(strResponse).success;
                }
                else
                {
                    return false;
                }
            }
        }

        private enum ApiKeyPair
        {
            secret,
            response,
            remoteip
        }

        private class GoogleResponse
        {
            public bool success { get; set; }

            public string challenge_ts { get; set; }

            public string hostname { get; set; }

            [JsonProperty("error-codes")]
            public string[] error_codes { get; set; }
        }
    }
}