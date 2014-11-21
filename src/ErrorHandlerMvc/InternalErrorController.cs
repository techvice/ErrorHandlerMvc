using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    public class InternalErrorController : IInternalErrorController
    {
        public void Execute(RequestContext requestContext)
        {
            var exception = requestContext.RouteData.DataTokens[Constants.Exception] as Exception;
            var controllerName = requestContext.RouteData.DataTokens[Constants.Controller] as string;
            var actionName = requestContext.RouteData.DataTokens[Constants.Action] as string;
			
			Controller controller = new FakeController();
			var context = new ControllerContext(requestContext, controller);
			controller.ControllerContext = context;
			
            var viewResult = new InternalErrorViewResult(exception, controllerName, actionName);
            viewResult.ExecuteResult(context);
        }

        public ActionResult Error(Exception ex, string controllerName, string actionName)
        {
            return new InternalErrorViewResult(ex, controllerName, actionName);
        }

        // ControllerContext requires an object that derives from ControllerBase.
        // InternalErrorController does not do this.
        // So the easiest workaround is this FakeController.
        private class FakeController : Controller
        {
        }
    }

    public interface IInternalErrorController : IController
    {
        ActionResult Error(Exception ex, string controllerName, string actionName);
    }
}