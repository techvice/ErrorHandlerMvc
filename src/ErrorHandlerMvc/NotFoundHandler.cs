using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    public class NotFoundHandler : IHttpHandler
    {
        private static Func<RequestContext, IController> _createNotFoundController = context => (IController)new NotFoundController();

        public static Func<RequestContext, IController> CreateNotFoundController
        {
            get
            {
                return _createNotFoundController;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _createNotFoundController = value;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        static NotFoundHandler()
        {
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(new HttpContextWrapper(context));
        }

        private void ProcessRequest(HttpContextBase context)
        {
            RequestContext requestContext = CreateRequestContext(context);
            _createNotFoundController(requestContext).Execute(requestContext);
        }

        private RequestContext CreateRequestContext(HttpContextBase context)
        {
            return new RequestContext(context, new RouteData
            {
                Values =
                {
                    {
                        "controller",
                        (object) "NotFound"
                    }
                }
            });
        }

        private class FakeController : Controller
        {
        }
    }
}