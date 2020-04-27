using Microsoft.AspNetCore.Authorization;

namespace LqdOnlineHub.Attributes
{
    public class AuthorizeAdministratorAttribute : AuthorizeAttribute
    {
        public new string Roles = ApplicationRoles.Administrator;
    }

    public class AuthorizeYouthUnionAttribute : AuthorizeAttribute
    {
        public new string Roles = ApplicationRoles.YouthUnion;
    }

    public class AuthorizeBranchSecAttribute : AuthorizeAttribute
    {
        public new string Roles = ApplicationRoles.BranchSec;
    }
}
