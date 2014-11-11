using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof(ErrorHandlerMvc.PreApplicationStarter), "Start")]

namespace ErrorHandlerMvc
{
    /// <summary>
    /// Runs at web application start.
    /// </summary>
    public class PreApplicationStarter
    {
        private static bool _started;

        public static void Start()
        {
            if (_started) return; // Only start once.
            _started = true;

            DynamicModuleUtility.RegisterModule(typeof(InstallerModule));
        }
    }
}