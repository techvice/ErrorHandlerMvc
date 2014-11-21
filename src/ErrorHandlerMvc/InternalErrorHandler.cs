using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class InternalErrorHandler : IHttpHandler
    {
        private static Func<RequestContext, IInternalErrorController> _createInternalErrorController =
            context => (IInternalErrorController) new InternalErrorController();

        public static Func<RequestContext, IInternalErrorController> CreateInternalErrorController
        {
            get { return _createInternalErrorController; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _createInternalErrorController = value;
            }
        }

        public bool IsReusable
        {
            get { return false; }
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

        private static RequestContext CreateRequestContext(HttpContextBase context)
        {
            return new RequestContext(context, new RouteData {Values = {{"controller", (object) "Error"}}});
        }

        private class FakeController : Controller
        {
        }
    }
}