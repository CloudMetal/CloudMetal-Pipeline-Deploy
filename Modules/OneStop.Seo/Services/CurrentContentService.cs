using System;
using Orchard;
using Orchard.Alias;
using Orchard.ContentManagement;

namespace Onestop.Seo.Services {
    public class CurrentContentService : ICurrentContentService {
        private readonly IContentManager _contentManager;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly IAliasService _aliasService;

        // Since this is an IDependency, the lifetime of such caching is just the request.
        private bool _contentWasChecked = false;
        private IContent _contentForRequest = null;

        public CurrentContentService(
            IContentManager contentManager,
            IWorkContextAccessor workContextAccessor,
            IAliasService aliasService) {
            _contentManager = contentManager;
            _workContextAccessor = workContextAccessor;
            _aliasService = aliasService;
        }

        public IContent GetContentForRequest() {
            if (_contentWasChecked) return _contentForRequest;

            _contentWasChecked = true;

            // Checking if the page we're currently on is a content item
            var itemRoute = _aliasService.Get(_workContextAccessor.GetContext().HttpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(1).Trim('/'));
            if (itemRoute == null) return null;

            var itemId = Convert.ToInt32(itemRoute["Id"]);
            _contentForRequest = _contentManager.Get(itemId);

            return _contentForRequest;
        }
    }
}