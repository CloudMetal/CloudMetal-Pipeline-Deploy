using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Onestop.Seo.Services;
using Orchard.Environment;
using Orchard.Mvc.Filters;
using Orchard.Tokens;
using Orchard.UI.Resources;

namespace Onestop.Seo.Filters {
    public class HomePageFilter : FilterProvider, IResultFilter {
        private readonly Work<ISeoSettingsManager> _seoSettingsManagerWork;
        private readonly Work<ICurrentContentService> _currentContentServiceWork;
        private readonly Work<ITokenizer> _tokenizerWork;
        private readonly Work<ISeoPageTitleBuilder> _pageTitleBuilderWork;
        private readonly Work<IResourceManager> _resourceManagerWork;

        public HomePageFilter(
            Work<ISeoSettingsManager> seoSettingsManagerWork,
            Work<ICurrentContentService> currentContentServiceWork,
            Work<ITokenizer> tokenizerWork,
            Work<ISeoPageTitleBuilder> pageTitleBuilderWork,
            Work<IResourceManager> resourceManagerWork) {
            _seoSettingsManagerWork = seoSettingsManagerWork;
            _currentContentServiceWork = currentContentServiceWork;
            _tokenizerWork = tokenizerWork;
            _pageTitleBuilderWork = pageTitleBuilderWork;
            _resourceManagerWork = resourceManagerWork;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext) {

        }

        public void OnResultExecuting(ResultExecutingContext filterContext) {
            if (!filterContext.HttpContext.Request.IsHomePage()) return;

            var globalSettings = _seoSettingsManagerWork.Value.GetGlobalSettings();


            if (!String.IsNullOrEmpty(globalSettings.HomeTitle)) {
                _pageTitleBuilderWork.Value.OverrideTitle(Tokenize(globalSettings.HomeTitle)); 
            }


            var resourceManager = _resourceManagerWork.Value;

            if (!String.IsNullOrEmpty(globalSettings.HomeDescription)) {
                resourceManager.SetMeta(new MetaEntry {
                    Name = "description",
                    Content = Tokenize(globalSettings.HomeDescription)
                }); 
            }

            if (!String.IsNullOrEmpty(globalSettings.HomeKeywords)) {
                resourceManager.SetMeta(new MetaEntry {
                    Name = "keywords",
                    Content = Tokenize(globalSettings.HomeKeywords)
                }); 
            }
        }

        private string Tokenize(string pattern) {
            Dictionary<string, object> replaceData  = null;
            var currentContent = _currentContentServiceWork.Value.GetContentForRequest();
            if (currentContent != null) replaceData = new Dictionary<string, object> { { "Content", currentContent } };

            return _tokenizerWork.Value.Replace(
                            pattern,
                            replaceData,
                            new ReplaceOptions { Encoding = ReplaceOptions.NoEncode });
        }
    }
}