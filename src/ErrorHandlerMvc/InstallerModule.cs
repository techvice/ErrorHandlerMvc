using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ErrorHandlerMvc
{
    class InstallerModule : IHttpModule
    {
        private static readonly object InstallerLock = new object();
        private static bool _installed;

        public void Init(HttpApplication application)
        {
            if (_installed)
                return;
            lock (InstallerLock)
            {
                if (_installed)
                    return;
                Install();
                _installed = true;
            }
        }

        private static void Install()
        {
            WrapControllerBuilder();
            RouteCollection routes = RouteTable.Routes;
            using (routes.GetWriteLock())
            {
                AddNotFoundRoute(routes);
                AddErrorRoute(routes);
                AddCatchAllRoute(routes);
            }
        }

        private static void WrapControllerBuilder()
        {
            ControllerBuilder.Current.SetControllerFactory(
                new ControllerFactoryWrapper(ControllerBuilder.Current.GetControllerFactory()));
        }

        private static void AddErrorRoute(RouteCollection routes)
        {
            var route = new Route("error",
                new RouteValueDictionary(new {controller = "InternalError", action = "Error",}),
                new RouteValueDictionary(new {incoming = new IncomingRequestRouteConstraint()}), new MvcRouteHandler());

            // Insert at start of route table. 
            //This means the application can still create another route like "{name}" that won't capture "/error".
            routes.Insert(0, route);
        }

        private static void AddNotFoundRoute(RouteCollection routes)
        {
            // To allow IIS to execute "/notfound" when requesting something which is disallowed,
            // such as /bin or /add_data.
            var route = new Route("notfound",
                new RouteValueDictionary(new {controller = "NotFound", action = "NotFound"}),
                new RouteValueDictionary(new {incoming = new IncomingRequestRouteConstraint()}), new MvcRouteHandler());
			
            // Insert at start of route table. 
            //This means the application can still create another route like "{name}" that won't capture "/notfound".
            routes.Insert(0, route);
        }

        private static void AddCatchAllRoute(RouteCollection routes)
        {
            routes.MapRoute("NotFound-Catch-All", "{*any}", new {controller = "NotFound", action = "NotFound"});
        }

        public void Dispose()
        {
        }

        private class IncomingRequestRouteConstraint : IRouteConstraint
        {
            public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
            {
                return routeDirection == RouteDirection.IncomingRequest;
            }
        }
    }
}
