using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace ErrorHandlerMvc
{
    public class ErrorHandlingAsyncControllerActionInvoker : AsyncControllerActionInvoker
    {
        protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor,
            string actionName)
        {
            var result = base.FindAction(controllerContext, controllerDescriptor, actionName);
            return result ?? new NotFoundActionDescriptor();
        }

        protected override ActionResult InvokeActionMethod(ControllerContext controllerContext, ActionDescriptor actionDescriptor,
            IDictionary<string, object> parameters)
        {
            object returnValue;
            try
            {
                returnValue = actionDescriptor.Execute(controllerContext, parameters);
            }
            catch (Exception ex)
            {
                string actionName = actionDescriptor.ActionName;
                actionDescriptor = new InternalErrorActionDescriptor();

                parameters.Add(Constants.Exception, ex);
                parameters.Add(Constants.Controller, controllerContext.Controller.GetType().Name);
                parameters.Add(Constants.Action, actionName);

                returnValue = actionDescriptor.Execute(controllerContext, parameters);
            }
            ActionResult result = CreateActionResult(controllerContext, actionDescriptor, returnValue);
            return result;
        }
    }
}