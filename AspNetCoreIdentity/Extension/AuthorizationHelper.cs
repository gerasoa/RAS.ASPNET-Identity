using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Extension
{
    public class NecessessaryPermission : IAuthorizationRequirement
    {
        public string Permission { get; set; }

        public NecessessaryPermission(string permission)
        {
            Permission = permission;           
        }
    }


    //When we have "policy.Requirements" in Startup.cs we have to create a Handle (AuthorizationHandler) to configure te rule
    public class NecessessaryPermissionHandler : AuthorizationHandler<NecessessaryPermission>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NecessessaryPermission requirement)
        {
            if(context.User.HasClaim(c => c.Type == "Permission" && c.Value.Contains(requirement.Permission)))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
