using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Mvc5OWINwithAD.Filters;

namespace Mvc5OWINwithAD.Models.Override
{
    /// <summary>
    /// 複寫用於設定pagetitle
    /// </summary>
    public class CustomerCreateActionInvoker: ControllerActionInvoker
    {
        protected override ActionResult CreateActionResult(ControllerContext controllerContext, ActionDescriptor actionDescriptor, object actionReturnValue)
        {
            var result = base.CreateActionResult(controllerContext, actionDescriptor, actionReturnValue);
            if (!controllerContext.IsChildAction && result is ViewResult)
            {
                var viewResult = result as ViewResult;
                if (!HasPageTitleAttribute(viewResult, actionDescriptor))
                {
                    HasPageTitleAttribute(viewResult, controllerContext.Controller.GetType());
                }
            }
            return result;
        }
        private bool HasPageTitleAttribute(ViewResult viewResult, ICustomAttributeProvider attrubteProvider)
        {
            //當有抓到自訂的Attribute [PageTitleAttribute] 時，就將它設定到ViewData
            var attr = attrubteProvider.GetCustomAttributes(typeof(PageTitleAttribute), true).FirstOrDefault()
                       as PageTitleAttribute;

            if (attr != null)
            {
                //使用自訂Attribute的GetTitle取得Title
                viewResult.ViewData["PageTitle"] = attr.GetTitle(viewResult.ViewData.Model);
                return true;
            }
            return false;
        }

    }
}