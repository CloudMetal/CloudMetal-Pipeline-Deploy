using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace Onestop.Seo {
    public class Permissions : IPermissionProvider {
        public static readonly Permission ManageSeo = new Permission { Description = "Manage SEO settings", Name = "ManageSeo" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions() {
            return new[] {
                ManageSeo
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes() {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] { ManageSeo }
                }
            };
        }
    }
}