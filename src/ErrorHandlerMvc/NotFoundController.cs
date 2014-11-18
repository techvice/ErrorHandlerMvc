using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    public class NotFoundController : IController, INotFoundController
    {
        public void Execute(RequestContext requestContext)
        {
            ExecuteNotFound(requestContext);
        }

        public void ExecuteNotFound(RequestContext requestContext)
        {
			Controller controller = new FakeController();
			ControllerContext context = new ControllerContext(requestContext, controller);
			controller.ControllerContext = context;
            new NotFoundViewResult().ExecuteResult(context);
        }

        public ActionResult NotFound()
        {
            return new NotFoundViewResult();
        }

        // ControllerContext requires an object that derives from ControllerBase.
        // NotFoundController does not do this.
        // So the easiest workaround is this FakeController.
        private class FakeController : Controller
        {
        }
    }

    public interface INotFoundController : IController
    {
        ActionResult NotFound();
    }
}