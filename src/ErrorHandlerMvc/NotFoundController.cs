using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    public class NotFoundController : ControllerBase, INotFoundController
	{
		protected override void ExecuteCore()
		{
			new NotFoundViewResult().ExecuteResult(ControllerContext);
		}

        public ActionResult NotFound()
        {
            return new NotFoundViewResult();
        }
    }

    public interface INotFoundController : IController
    {
        ActionResult NotFound();
    }
}