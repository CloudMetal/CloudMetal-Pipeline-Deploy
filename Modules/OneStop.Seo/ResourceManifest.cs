using Orchard.UI.Resources;

namespace Onestop.Seo {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();

            manifest.DefineScript("OnestopSeo_Admin").SetUrl("SeoAdmin.js").SetDependencies(new string[] { "jQuery" });
            manifest.DefineStyle("OnestopSeo_Admin").SetUrl("onestop-seo-admin.css");
        }
    }
}