using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LqdOnlineHub.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetJobTitle(this ClaimsPrincipal principal)
        {
            foreach (var item in principal.Claims)
            {
                if (item.Type == GeneralConstants.JOBTITLE_CLAIM_NAME) return item.Value;
            }

            return string.Empty;
        }
    }
}
