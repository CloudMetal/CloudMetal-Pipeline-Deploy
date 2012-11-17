using Orchard.Core.Contents.ViewModels;

namespace Onestop.Seo.ViewModels {
    public class RewriterViewModel : ListContentsViewModel {
        public string RewriterType { get; set; }

        /// <summary>
        /// Search query
        /// </summary>
        public string Q { get; set; }
    }
}