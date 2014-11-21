using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    public class InternalErrorActionDescriptor : ActionDescriptor
    {
        public override object Execute(ControllerContext controllerContext, IDictionary<string, object> parameters)
        {
            IInternalErrorController controller =
                InternalErrorHandler.CreateInternalErrorController(controllerContext.RequestContext);
            controllerContext.RouteData.Values["action"] = "Error";
            return controller.Error((Exception) parameters[Constants.Exception],
                (string) parameters[Constants.Controller], (string) parameters[Constants.Action]);
        }

        public override ParameterDescriptor[] GetParameters()
        {
            return new ParameterDescriptor[] {};
        }

        public override string ActionName
        {
            get { return "Error"; }
        }

        public override ControllerDescriptor ControllerDescriptor
        {
            get { return new ReflectedControllerDescriptor(typeof (InternalErrorController)); }
        }
    }
}