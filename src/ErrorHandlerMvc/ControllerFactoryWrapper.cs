using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace ErrorHandlerMvc
{
    class ControllerFactoryWrapper : IControllerFactory
    {
        private readonly IControllerFactory _factory;

        public ControllerFactoryWrapper(IControllerFactory factory)
        {
            _factory = factory;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            try
            {
                IController controller = _factory.CreateController(requestContext, controllerName);
                WrapControllerActionInvoker(controller);
                return controller;
            }
            catch (HttpException ex)
            {
                return ex.GetHttpCode() == 404
                    ? NotFoundHandler.CreateNotFoundController(requestContext)
                    : CreateInternalErrorController(requestContext, ex, controllerName);
            }
            catch (Exception ex)
            {
                return CreateInternalErrorController(requestContext, ex, controllerName);
            }
        }

        private static IController CreateInternalErrorController(RequestContext requestContext, Exception ex, string controllerName)
        {
            requestContext.RouteData.DataTokens[Constants.Exception] = ex;
            requestContext.RouteData.DataTokens[Constants.Controller] = controllerName;
            requestContext.RouteData.DataTokens[Constants.Action] = "Unknown. Error during controller creation";
            return InternalErrorHandler.CreateInternalErrorController(requestContext);
        }

        private static void WrapControllerActionInvoker(IController iController)
        {
            var controller = iController as Controller;
            if (controller == null)
                return;
            controller.ActionInvoker = ActionInvokerSelector.Current(controller.ActionInvoker);
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return _factory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            _factory.ReleaseController(controller);
        }
    }
}