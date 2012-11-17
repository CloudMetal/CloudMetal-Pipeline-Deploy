using Onestop.Seo.Models;
using Orchard;
using Orchard.ContentManagement;

namespace Onestop.Seo.Services {
    public interface ISeoSettingsManager : IDependency {
        ISeoGlobalSettings GetGlobalSettings();
        dynamic UpdateSettings(IUpdateModel updater);
    }
}
