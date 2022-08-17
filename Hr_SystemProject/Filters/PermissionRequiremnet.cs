using Microsoft.AspNetCore.Authorization;

namespace HR_SystemProject.Filters
{
    public class PermissionRequiremnet:IAuthorizationRequirement
    {
        public string Permission { get; private set; }
        public PermissionRequiremnet(string permission)
        {
            Permission = permission;
        }
    }
}
