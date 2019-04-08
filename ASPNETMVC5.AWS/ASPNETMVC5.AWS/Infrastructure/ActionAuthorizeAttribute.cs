using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNETMVC5.AWS.Infrastructure
{
    public class ActionAuthorizeAttribute : ActionFilterAttribute
    {       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase req = filterContext.HttpContext.Request;
            HttpResponseBase res = filterContext.HttpContext.Response;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (filterContext.HttpContext.User.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectResult("/Home/Index");
                return;
            }

            if (filterContext.IsChildAction)
                return;

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (IsAnonymousRequest(filterContext))
            {
                base.OnActionExecuting(filterContext);
                return;
            }


            var userName = filterContext.HttpContext.User.Identity.Name;

            if (String.IsNullOrEmpty(userName))
            {
                filterContext.Result = new RedirectResult("/Home/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Check if this is an anonymous request
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private static bool IsAnonymousRequest(ActionExecutingContext filterContext)
        {
            var attributes = GetAllowAnonymousAttributes(filterContext.ActionDescriptor);

            return attributes != null && attributes.Any();
        }


        /// <summary>
        /// Get allow anonymous attributes
        /// </summary>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        private static IEnumerable<AllowAnonymousAttribute> GetAllowAnonymousAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true))
                .OfType<AllowAnonymousAttribute>();
        }
    }
}