using LqdOnlineHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LqdOnlineHub.Policies
{
    public class SchoolMemberRequirement : IAuthorizationRequirement
    {
    }

    public class SchoolMemberHandler : AuthorizationHandler<SchoolMemberRequirement>
    {
        private readonly AppSettings appSettings;

        public SchoolMemberHandler(IOptions<AppSettings> settings)
        {
            appSettings = settings.Value;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SchoolMemberRequirement requirement)
        {
            foreach (var cl in context.User.Claims)
            {
                if (cl.Type == GeneralConstants.DEPARTMENT_CLAIM_NAME)
                {
                    if (cl.Value == appSettings.DepartmentId)
                    {
                        context.Succeed(requirement);
                    }

                    break;
                }
            }

            return Task.CompletedTask;
        }
    }
}
