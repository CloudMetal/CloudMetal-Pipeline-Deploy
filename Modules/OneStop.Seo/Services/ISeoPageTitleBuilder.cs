using Orchard.UI.PageTitle;

namespace Onestop.Seo.Services {
    public interface ISeoPageTitleBuilder : IPageTitleBuilder {
        /// <summary>
        /// If an override is set for the title, it will be the title's value regardless of anything else
        /// </summary>
        void OverrideTitle(string title);
    }
}
