using System;
using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    internal static class ActionInvokerSelector
    {
        static ActionInvokerSelector()
        {
            var mvcVersion = typeof (Controller).Assembly.GetName().Version.Major;
            Current = mvcVersion <= 3 ? Mvc3Invoker : Mvc4Invoker;
        }

        private static readonly Func<IActionInvoker, IActionInvoker> Mvc3Invoker =
            originalActionInvoker => new ActionInvokerWrapper(originalActionInvoker);

        private static readonly Func<IActionInvoker, IActionInvoker> Mvc4Invoker =
            originalActionInvoker => new ErrorHandlingAsyncControllerActionInvoker();

        public static Func<IActionInvoker, IActionInvoker> Current { get; private set; }
    }
}