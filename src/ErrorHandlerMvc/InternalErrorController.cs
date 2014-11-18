using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    public class InternalErrorController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            var exception = requestContext.RouteData.DataTokens[Constants.Exception] as Exception;
            var controllerName = requestContext.RouteData.DataTokens[Constants.Controller] as string;
            var actionName = requestContext.RouteData.DataTokens[Constants.Action] as string;
			
			Controller controller = new FakeController();
			ControllerContext context = new ControllerContext(requestContext, controller);
			controller.ControllerContext = context;
			
            var viewResult = new InternalErrorViewResult(exception, controllerName, actionName);
            viewResult.ExecuteResult(context);
        }

        private class FakeController : Controller
        {
        }
    }
}