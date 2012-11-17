using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Onestop.Seo {
    public class Routes : IRouteProvider {
        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes()) {
                routes.Add(routeDescriptor);
            }
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[]
                {
                    new RouteDescriptor
                        {
                            Priority = 100, 
                            Route =
                                new Route(
                                "Onestop.Seo/Rewriter/{rewriterType}/{id}", 
                                new RouteValueDictionary
                                    {
                                        { "area", "Onestop.Seo" }, 
                                        { "controller", "Admin" }, 
                                        { "action", "Rewriter" }
                                    }, 
                                new RouteValueDictionary(), 
                                new RouteValueDictionary { { "area", "Onestop.Seo" } }, 
                                new MvcRouteHandler())
                        }
                };
        }
    }
}