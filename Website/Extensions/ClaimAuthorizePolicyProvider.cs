using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.Extensions
{
    public class ClaimAuthorizePolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions Options;

        public ClaimAuthorizePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            if(options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Value;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult(Options.DefaultPolicy);
        }

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            AuthorizationPolicy policy = Options.GetPolicy(policyName);

            if(policy == null)
            {
                string[] resourceValues = policyName.Split(new string[] { "=====" }, StringSplitOptions.RemoveEmptyEntries);
                Options.AddPolicy(policyName, builder =>
                {
                    builder.RequireClaim(resourceValues[0], new string[] { resourceValues[1] });
                });
            }

            return Task.FromResult(Options.GetPolicy(policyName));
        }
    }
}
