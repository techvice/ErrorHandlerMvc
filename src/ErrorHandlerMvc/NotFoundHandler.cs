using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NotFoundHandler : IHttpHandler
    {
        private static Func<RequestContext, INotFoundController> _createNotFoundController =
            context => new NotFoundController();

        public static Func<RequestContext, INotFoundController> CreateNotFoundController
        {
            get { return _createNotFoundController; }
            set
            {
                if (value == null) throw new ArgumentNullException();
                _createNotFoundController = value;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        private void ProcessRequest(HttpContextBase context)
        {
            var requestContext = CreateRequestContext(context);
            var controller = _createNotFoundController(requestContext);
            controller.Execute(requestContext);
        }

        private RequestContext CreateRequestContext(HttpContextBase context)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "NotFound");
            var requestContext = new RequestContext(context, routeData);
            return requestContext;
        }

        public bool IsReusable
        {
            get { return false; }
        }

        // ControllerContext requires an object that derives from ControllerBase.
        private class FakeController : Controller
        {
        }
    }
}