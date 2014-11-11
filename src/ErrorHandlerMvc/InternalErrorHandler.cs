using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    public class InternalErrorHandler : IHttpHandler
    {
        private static Func<RequestContext, IController> _createInternalErrorController = context => (IController)new InternalErrorController();

        public static Func<RequestContext, IController> CreateInternalErrorController
        {
            get
            {
                return _createInternalErrorController;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _createInternalErrorController = value;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        private void ProcessRequest(HttpContextBase context)
        {
            RequestContext requestContext = CreateRequestContext(context);
            _createInternalErrorController(requestContext).Execute(requestContext);
        }

        private RequestContext CreateRequestContext(HttpContextBase context)
        {
            return new RequestContext(context, new RouteData
            {
                Values =
                {
                    {
                        "controller",
                        (object) "Error"
                    }
                }
            });
        }

        private class FakeController : Controller
        {
        }
    }
}