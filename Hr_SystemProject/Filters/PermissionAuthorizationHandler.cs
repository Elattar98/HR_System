﻿using Microsoft.AspNetCore.Authorization;

namespace HR_SystemProject.Filters
{
    public class PermissionAuthorizationHandler: AuthorizationHandler<PermissionRequiremnet>
    {
        public PermissionAuthorizationHandler()
        {

        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequiremnet requirement)
        {
            if (context.User == null)
                return;

            var canAccess = context.User.Claims.Any(c => c.Type ==
            "Permission" && c.Value == requirement.Permission && c.Issuer == "LOCAL AUTHORITY");
            
            if(canAccess)
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
