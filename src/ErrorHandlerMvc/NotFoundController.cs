using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    public class NotFoundController : IController
    {
        public void Execute(RequestContext requestContext)
        {
            var viewResult = new NotFoundViewResult();
            viewResult.ExecuteResult(new ControllerContext(requestContext, new FakeController()));
        }

        private class FakeController : Controller
        {
        }
    }
}