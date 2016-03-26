using System;
using System.Web.Mvc;

namespace MyMVCTest04.Controllers
{
    public class CountExecutionTimeAttribute : ActionFilterAttribute
    {
        DateTime startTime = DateTime.Now;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            startTime = DateTime.Now;
        }



        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            DateTime endTime = DateTime.Now;
            filterContext.Controller.ViewBag.execTime = ((TimeSpan)(endTime - startTime)).TotalMilliseconds.ToString();
        }
        
    }
}