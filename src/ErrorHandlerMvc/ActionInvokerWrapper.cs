using System;
using System.Web;
using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    /// <summary>
    /// Wraps another IActionInvoker except it handles errors and invokes the appropriate controller instead.
    /// </summary>
    class ActionInvokerWrapper : IActionInvoker
    {
        private readonly IActionInvoker _actionInvoker;

        public ActionInvokerWrapper(IActionInvoker actionInvoker)
        {
            _actionInvoker = actionInvoker;
        }

        public bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            try
            {
                return _actionInvoker.InvokeAction(controllerContext, actionName);
            }
            catch (HttpException ex)
            {
                if (ex.GetHttpCode() == 404)
                {
                    ExecuteNotFoundControllerAction(controllerContext);
                    return true;
                }

                ExecuteInternalErrorControllerAction(controllerContext, ex, actionName);
                return true;
            }
            catch (Exception ex)
            {
                ExecuteInternalErrorControllerAction(controllerContext, ex, actionName);
                return true;
            }
        }

        private static void ExecuteNotFoundControllerAction(ControllerContext controllerContext)
        {
            (NotFoundHandler.CreateNotFoundController == null
                ? new NotFoundController()
                : NotFoundHandler.CreateNotFoundController(controllerContext.RequestContext) ?? new NotFoundController())
                .Execute(controllerContext.RequestContext);
        }

        private static void ExecuteInternalErrorControllerAction(ControllerContext controllerContext, Exception ex, string actionName)
        {
            controllerContext.RequestContext.RouteData.DataTokens[Constants.Exception] = ex;
            controllerContext.RequestContext.RouteData.DataTokens[Constants.Controller] = controllerContext.Controller.GetType().Name;
            controllerContext.RequestContext.RouteData.DataTokens[Constants.Action] = actionName;

            (InternalErrorHandler.CreateInternalErrorController == null
                ? new InternalErrorController()
                : InternalErrorHandler.CreateInternalErrorController(controllerContext.RequestContext) ?? new InternalErrorController())
                .Execute(controllerContext.RequestContext);
        }
    }
}
