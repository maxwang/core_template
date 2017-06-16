using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Extensions
{
    //public class AuthorizeClaimAttribute : IAsyncAuthorizationFilter, IFilterFactory
    public class ClaimAuthorizeAttribute : AuthorizeAttribute
    {
        //private readonly string ClaimType;
        //private readonly string ClaimValue;
        private readonly string ResourceName;
        public ClaimAuthorizeAttribute(string claimType, string claimValue)
        {
            if(string.IsNullOrEmpty(claimType) || string.IsNullOrEmpty(claimValue))
            {
                throw new ArgumentNullException("Claim Type and Value are required.");
            }
            //ClaimType = claimType;
            //ClaimValue = claimValue;
            ResourceName = string.Format("{0}====={1}", claimType, claimValue);
            Policy = ResourceName;
        }


    }
    
}
