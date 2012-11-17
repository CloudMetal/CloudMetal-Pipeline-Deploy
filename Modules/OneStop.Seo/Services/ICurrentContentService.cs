using Orchard;
using Orchard.ContentManagement;

namespace Onestop.Seo.Services {
    // Better name?
    public interface ICurrentContentService : IDependency {
        IContent GetContentForRequest();
    }
}
