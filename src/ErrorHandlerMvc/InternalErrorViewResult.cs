using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ErrorHandlerMvc
{
    public class InternalErrorViewResult : HttpStatusCodeResult
    {
        public string ViewName { get; set; }

        public ViewDataDictionary ViewData { get; set; }

        public InternalErrorViewResult(Exception ex, string controllerName, string actionName)
            : base(HttpStatusCode.InternalServerError)
        {
            ViewName = "Error";
            ViewData = new ViewDataDictionary
            {
                Model =
                    new HandleErrorInfo(ex, string.IsNullOrEmpty(controllerName) ? "Unknown" : controllerName,
                        string.IsNullOrEmpty(actionName) ? "Unknown" : actionName)
            };
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            HttpRequestBase request = context.HttpContext.Request;
            ViewData["RequestedUrl"] = GetRequestedUrl(request);
            ViewData["ReferrerUrl"] = GetReferrerUrl(request, request.Url.OriginalString);
            response.TrySkipIisCustomErrors = true;
            var viewResult1 = new ViewResult { ViewName = ViewName, ViewData = ViewData };
            ViewResult viewResult2 = viewResult1;
            response.Clear();
            viewResult2.ExecuteResult(context);
        }

        private string GetRequestedUrl(HttpRequestBase request)
        {
            return request.AppRelativeCurrentExecutionFilePath != "~/error"
                ? request.Url.OriginalString
                : ExtractOriginalUrlFromExecuteUrlModeErrorRequest(request.Url);
        }

        private static string GetReferrerUrl(HttpRequestBase request, string url)
        {
            if (!(request.UrlReferrer != null) || request.UrlReferrer.OriginalString == url)
                return null;
            return request.UrlReferrer.OriginalString;
        }

        private static string ExtractOriginalUrlFromExecuteUrlModeErrorRequest(Uri url)
        {
            int num = url.Query.IndexOf(';');
            if (0 <= num && num < url.Query.Length - 1)
                return url.Query.Substring(num + 1);
            return url.ToString();
        }
    }
}