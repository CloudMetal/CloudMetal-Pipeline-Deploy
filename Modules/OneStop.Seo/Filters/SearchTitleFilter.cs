using System;
using System.Web.Mvc;
using Onestop.Seo.Services;
using Orchard.Environment;
using Orchard.Mvc.Filters;
using Orchard.Tokens;

namespace Onestop.Seo.Filters {
    public class SearchTitleFilter : FilterProvider, IActionFilter {
        private readonly Work<ISeoSettingsManager> _seoSettingsManagerWork;
        private readonly Work<ITokenizer> _tokenizerWork;
        private readonly Work<ISeoPageTitleBuilder> _pageTitleBuilderWork;

        public SearchTitleFilter(
            Work<ISeoSettingsManager> seoSettingsManagerWork,
            Work<ITokenizer> tokenizerWork,
            Work<ISeoPageTitleBuilder> pageTitleBuilderWork) {
            _seoSettingsManagerWork = seoSettingsManagerWork;
            _tokenizerWork = tokenizerWork;
            _pageTitleBuilderWork = pageTitleBuilderWork;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext) {
            var routeValues = filterContext.HttpContext.Request.RequestContext.RouteData.Values;
            if ((string)routeValues["area"] != "Orchard.Search" 
                || (string)routeValues["controller"] != "search" 
                || (string)routeValues["action"] != "index") return;

            var titlePattern = _seoSettingsManagerWork.Value.GetGlobalSettings().SearchTitlePattern;
            if (String.IsNullOrEmpty(titlePattern)) return;

            var title = _tokenizerWork.Value.Replace(
                        titlePattern,
                        null,
                        new ReplaceOptions { Encoding = ReplaceOptions.NoEncode });

            _pageTitleBuilderWork.Value.OverrideTitle(title);
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) {
        }
    }
}