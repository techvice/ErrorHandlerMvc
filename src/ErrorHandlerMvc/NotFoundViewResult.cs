using System;
using System.Web;
using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    /// <summary>
    /// Renders a view called "NotFound" and sets the response status code to 404.
    /// View data is assigned for "RequestedUrl" and "ReferrerUrl".
    /// </summary>
    public class NotFoundViewResult : HttpNotFoundResult
    {
        /// <summary>
        /// The name of the view to render. Defaults to "NotFound".
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// The view data passed to the NotFound view.
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }
        public NotFoundViewResult()
        {
            ViewName = "NotFound";
            ViewData = new ViewDataDictionary();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            HttpRequestBase request = context.HttpContext.Request;
            ViewData["RequestedUrl"] = GetRequestedUrl(request);
            ViewData["ReferrerUrl"] = GetReferrerUrl(request, request.Url.OriginalString);
            
            response.StatusCode = 404;
            // Prevent IIS7 from overwriting our error page!
            response.TrySkipIisCustomErrors = true;
            var viewResult1 = new ViewResult {ViewName = ViewName, ViewData = ViewData};
            ViewResult viewResult2 = viewResult1;
            response.Clear();
            viewResult2.ExecuteResult(context);
        }

        private string GetRequestedUrl(HttpRequestBase request)
        {
            return request.AppRelativeCurrentExecutionFilePath != "~/notfound"
                ? request.Url.OriginalString
                : ExtractOriginalUrlFromExecuteUrlModeErrorRequest(request.Url);
        }

        private string GetReferrerUrl(HttpRequestBase request, string url)
        {
            if (!(request.UrlReferrer != null) || request.UrlReferrer.OriginalString == url)
                return null;
            return request.UrlReferrer.OriginalString;
        }

        /// <summary>
        /// Handles the case when a web.config &lt;error statusCode="404" path="/notfound" responseMode="ExecuteURL" /&gt; is triggered.
        /// The original URL is passed via the querystring.
        /// </summary>
        private string ExtractOriginalUrlFromExecuteUrlModeErrorRequest(Uri url)
        {
            // Expected format is "?404;http://hostname.com/some/path"
			int num = url.Query.IndexOf(';');
            if (0 <= num && num < url.Query.Length - 1)
                return url.Query.Substring(num + 1);

            // Unexpected format, so just return the full URL!
            return url.ToString();
        }
    }
}