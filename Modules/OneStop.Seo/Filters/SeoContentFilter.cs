using Onestop.Seo.Models;
using Onestop.Seo.Services;
using Orchard.ContentManagement;
using Orchard.Environment;
using Orchard.Mvc.Filters;
using Orchard.UI.Resources;
using System;
using System.Web.Mvc;

namespace Onestop.Seo.Filters {
    public class SeoContentFilter : FilterProvider, IResultFilter {
        private readonly Work<ISeoSettingsManager> _seoSettingsManagerWork;
        private readonly Work<ICurrentContentService> _currentContentServiceWork;
        private readonly Work<ISeoService> _seoServiceWork;
        private readonly Work<ISeoPageTitleBuilder> _pageTitleBuilderWork;
        private readonly Work<IResourceManager> _resourceManagerWork;

        public SeoContentFilter(
            Work<ISeoSettingsManager> seoSettingsManagerWork,
            Work<ICurrentContentService> currentContentServiceWork,
            Work<ISeoService> seoServiceWork,
            Work<ISeoPageTitleBuilder> pageTitleBuilderWork,
            Work<IResourceManager> resourceManagerWork) {
            _seoSettingsManagerWork = seoSettingsManagerWork;
            _currentContentServiceWork = currentContentServiceWork;
            _seoServiceWork = seoServiceWork;
            _pageTitleBuilderWork = pageTitleBuilderWork;
            _resourceManagerWork = resourceManagerWork;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {
        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            // Don't run on admin
            if (Orchard.UI.Admin.AdminFilter.IsApplied(filterContext.RequestContext)) return;

            if (filterContext.HttpContext.Request.IsHomePage()) return;


            var item = _currentContentServiceWork.Value.GetContentForRequest();
            if (item == null) return;

            if (!item.Has<SeoPart>()) return;
            var seoPart = item.As<SeoPart>();

            var title = !String.IsNullOrEmpty(seoPart.TitleOverride) ? seoPart.TitleOverride : seoPart.GeneratedTitle;
            if (!String.IsNullOrEmpty(title)) _pageTitleBuilderWork.Value.OverrideTitle(title);


            var description = !String.IsNullOrEmpty(seoPart.DescriptionOverride) ? seoPart.DescriptionOverride : seoPart.GeneratedDescription;
            if (!String.IsNullOrEmpty(description)) {
                _resourceManagerWork.Value.SetMeta(new MetaEntry {
                    Name = "description",
                    Content = description
                });
            }

            var keywords = !String.IsNullOrEmpty(seoPart.KeywordsOverride) ? seoPart.KeywordsOverride : seoPart.GeneratedKeywords;
            if (!String.IsNullOrEmpty(keywords)) {
                _resourceManagerWork.Value.SetMeta(new MetaEntry {
                    Name = "keywords",
                    Content = keywords
                });
            }
        }
    }
}