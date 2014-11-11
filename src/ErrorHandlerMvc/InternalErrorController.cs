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
            var viewResult = new InternalErrorViewResult(exception, controllerName, actionName);
            viewResult.ExecuteResult(new ControllerContext(requestContext, new FakeController()));
        }

        private class FakeController : Controller
        {
        }
    }
}