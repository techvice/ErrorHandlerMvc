using System;
using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    public class InternalErrorController : ControllerBase, IInternalErrorController
    {
	    protected override void ExecuteCore()
		{
			var exception = ControllerContext.RequestContext.RouteData.DataTokens[Constants.Exception] as Exception;
			var controllerName = ControllerContext.RequestContext.RouteData.DataTokens[Constants.Controller] as string;
			var actionName = ControllerContext.RequestContext.RouteData.DataTokens[Constants.Action] as string;

			var viewResult = new InternalErrorViewResult(exception, controllerName, actionName);
			viewResult.ExecuteResult(ControllerContext);
	    }

	    public ActionResult Error(Exception ex, string controllerName, string actionName)
        {
            return new InternalErrorViewResult(ex, controllerName, actionName);
        }
    }

    public interface IInternalErrorController : IController
    {
        ActionResult Error(Exception ex, string controllerName, string actionName);
    }
}