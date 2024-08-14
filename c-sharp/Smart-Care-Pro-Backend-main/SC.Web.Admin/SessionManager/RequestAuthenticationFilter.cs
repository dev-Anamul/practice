using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SC.Web.Admin.SessionManager
{
    public class RequestAuthenticationFilter : ActionFilterAttribute
    {
        /// 

        /// OnActionExecuting
        /// 

        /// 
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var session = context.HttpContext.Session.Get("CurrentAdmin");
            if (session == null)
            {

                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
                return;
            }
            await base.OnActionExecutionAsync(context, next);
        }
     
    }
}
