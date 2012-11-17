using JetBrains.Annotations;
using Onestop.Seo.Models;
using Onestop.Seo.Services;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Environment;

namespace Onestop.Seo.Handlers {
    [UsedImplicitly]
    public class SeoGlobalSettingsPartHandler : ContentHandler {
        public SeoGlobalSettingsPartHandler(
            IRepository<SeoGlobalSettingsPartRecord> repository,
            Work<ISeoService> seoServiceWork) {
            Filters.Add(new ActivatingFilter<SeoGlobalSettingsPart>("SeoSettings"));
            Filters.Add(StorageFilter.For(repository));

            OnActivated<SeoGlobalSettingsPart>((context, part) => {
                part.SeoContentTypesField.Loader(() => seoServiceWork.Value.ListSeoContentTypes());
            });
        }
    }
}