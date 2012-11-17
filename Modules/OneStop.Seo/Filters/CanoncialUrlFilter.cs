using System.Web.Mvc;
using Onestop.Seo.Services;
using Orchard.ContentManagement;
using Orchard.Environment;
using Orchard.Mvc.Filters;
using Orchard.UI.Resources;

namespace Onestop.Seo.Filters {
    public class CanoncialUrlFilter : FilterProvider, IResultFilter {
        private readonly Work<ISeoSettingsManager> _seoSettingsManagerWork;
        private readonly Work<ICurrentContentService> _currentContentServiceWork;
        private readonly Work<IContentManager> _contentManagerWork;
        private readonly Work<IResourceManager> _resourceManagerWork;

        public CanoncialUrlFilter(
            Work<ISeoSettingsManager> seoSettingsManagerWork,
            Work<ICurrentContentService> currentContentServiceWork,
            Work<IContentManager> contentManagerWork,
            Work<IResourceManager> resourceManagerWork) {
            _seoSettingsManagerWork = seoSettingsManagerWork;
            _currentContentServiceWork = currentContentServiceWork;
            _contentManagerWork = contentManagerWork;
            _resourceManagerWork = resourceManagerWork;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            // Don't run on admin
            if (Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext)) return;

            if (!_seoSettingsManagerWork.Value.GetGlobalSettings().EnableCanonicalUrls) return;

            // If the page we're currently on is a content item, produce a canonical url for it
            var item = _currentContentServiceWork.Value.GetContentForRequest();
            if (item == null) return;
            _resourceManagerWork.Value.RegisterLink(new LinkEntry {
                Rel = "canonical",
                Href = new UrlHelper(filterContext.RequestContext).RouteUrl(_contentManagerWork.Value.GetItemMetadata(item).DisplayRouteValues)
            });
        }
    }
}