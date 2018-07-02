using System.Web.Mvc;

namespace DeltaX.Models
{
    public class CustomErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            
            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var AreaName = (string)filterContext.RouteData.DataTokens["area"];

            var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            ErrorLog.WriteError(filterContext.Exception + " --- Controller --- " + controllerName + " -- Action Name -- " + actionName);
          
              filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Error.cshtml",
                    //MasterName = "~/Views/Shared/_Layout.cshtml",
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
              filterContext.ExceptionHandled = true;
        }
    }
}