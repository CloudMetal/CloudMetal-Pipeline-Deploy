using System.Web;

namespace Onestop.Seo {
    public static class RequestExtensions {
        public static bool IsHomePage(this HttpRequestBase request) {
            return request.AppRelativeCurrentExecutionFilePath == "~/";
        }
    }
}