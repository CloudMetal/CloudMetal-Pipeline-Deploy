using System.Collections.Generic;
using Onestop.Seo.Services;
using Orchard.Localization;
using Orchard.UI.Navigation;
using System.Linq;

namespace Onestop.Seo {
    public class AdminMenu : INavigationProvider {
        private readonly ISeoService _seoService;

        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public AdminMenu(ISeoService seoService) {
            _seoService = seoService;

            T = NullLocalizer.Instance;
        }

        public void GetNavigation(NavigationBuilder builder) {
            builder.Add(T("SEO"), "5", BuildMenu);
        }

        private void BuildMenu(NavigationItemBuilder menu) {
            menu.LinkToFirstChild(false); // See: http://orchard.codeplex.com/workitem/18807
            menu.Action("GlobalSettings", "Admin", new { area = "Onestop.Seo" }).Permission(Permissions.ManageSeo);


            var seoContentTypes = _seoService.ListSeoContentTypes();

            if (seoContentTypes.Count() != 0) {
                var rewriters = new List<Rewriter> {
                    new Rewriter { DisplayName = T("Title Tag Rewriter"), Type = "TitleRewriter" },
                    new Rewriter { DisplayName = T("Description Tag Rewriter"), Type = "DescriptionRewriter" },
                    new Rewriter { DisplayName = T("Keywords Tag Rewriter"), Type = "KeywordsRewriter" }
                };

                int i = 1;
                foreach (var rewriter in rewriters) {
                    menu.Add(rewriter.DisplayName, i.ToString(),
                       item => {
                           int l = 1;
                           foreach (var contentType in seoContentTypes) {
                               if (l == 1) {
                                   item.Action("Rewriter", "Admin", new { area = "Onestop.Seo", rewriterType = rewriter.Type, Id = contentType.Name });
                               }

                               item
                                   .Add(T(contentType.DisplayName), l.ToString(), tab => tab.Action("Rewriter", "Admin", new { area = "Onestop.Seo", rewriterType = rewriter.Type, Id = contentType.Name })
                                       .LocalNav()
                                       .Permission(Permissions.ManageSeo));

                               l++;
                           }
                       });

                    i++;
                } 
            }
        }

        private class Rewriter {
            public string Type { get; set; }
            public LocalizedString DisplayName { get; set; }
        }
    }
}