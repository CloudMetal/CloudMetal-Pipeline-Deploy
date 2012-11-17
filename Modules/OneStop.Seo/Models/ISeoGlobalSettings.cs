using Orchard.ContentManagement;

namespace Onestop.Seo.Models {
    public enum SeoParameterType {
        Title,
        Description,
        Keywords
    }

    public interface ISeoGlobalSettings : IContent {
        string HomeTitle { get; }
        string HomeDescription { get; }
        string HomeKeywords { get; }
        void SetSeoPattern(SeoParameterType type, string contentType, string pattern);
        string GetSeoPattern(SeoParameterType type, string contentType);
        string SearchTitlePattern { get; }
        bool EnableCanonicalUrls { get; }
    }
}