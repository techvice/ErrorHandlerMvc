using System;
using System.Web;
using System.Web.Mvc;
using ErrorHandlerMvc;

namespace SampleApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestNotFound(int id)
        {
            return new NotFoundViewResult();
        }

        public ActionResult FailThrowsExecption()
        {
            throw new Exception("An unanticipated exception occured in the code.");
        }

        public ActionResult TestInternalError()
        {
            return new InternalErrorViewResult(new Exception("Test of exception"), "Home", "TestInternalError");
        }

        public ActionResult FailError()
        {
            Response.Write("Attempt to write some content."); // Expecting the NotFoundViewResult to clear the response before sending its output.

            throw new HttpException(500, "Internal server error.");
        }
    }
}
