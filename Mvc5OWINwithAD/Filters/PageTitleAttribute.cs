using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Mvc5OWINwithAD.Filters
{   
    /// <summary>
     ///用attribute設定頁面標題
     /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PageTitleAttribute: Attribute
    {
        /// <summary> 設定Title </summary>
        private string Title { get; set; }

        /// <summary> 取得頁面Model內的任一屬性 </summary>
        private string PropertyName { get; set; }

        /// <summary> Format格式 </summary>
        private string Format { get; set; }

        private static BindingFlags _bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;

        /// <summary>
        /// 直接設定Title
        /// </summary>
        public PageTitleAttribute(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// 取得頁面的Model的屬性值，加上Format設定
        /// </summary>
        /// <param name="propertyName">屬性名稱</param>
        /// <param name="format">Format設定。範例：產品-{0} 介紹</param>
        public PageTitleAttribute(string propertyName, string format)
        {
            this.PropertyName = propertyName;
            this.Format = format;
        }

        /// <summary>
        /// 取得Title
        /// </summary>
        /// <param name="model">頁面的Model</param>
        public string GetTitle(object model)
        {
            if (this.Title != null)
                return this.Title;

            var title = String.Empty;

            if (model != null)
            {
                var prop = model.GetType().GetProperty(PropertyName, _bindingFlags);
                if (prop == null) throw new Exception("找不到屬性[" + PropertyName + "]");
                title = Convert.ToString(prop.GetValue(model, null));
            }
            return String.Format(this.Format, title);
        }

    }
}