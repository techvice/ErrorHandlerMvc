using System.Collections.Generic;
using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    public class NotFoundActionDescriptor : ActionDescriptor
    {
        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            INotFoundController notFound = NotFoundHandler.CreateNotFoundController(controllerContext.RequestContext);
            controllerContext.RouteData.Values["action"] = "NotFound";
            return notFound.NotFound();
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return new ParameterDescriptor[] {};
        }

        public override string ActionName
        {
            get { return "NotFound"; }
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get { return new ReflectedControllerDescriptor(typeof (NotFoundController)); }
        }
    }
}