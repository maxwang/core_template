using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Website.Extensions
{
    public class SMSUserManager<TUser> : UserManager<ApplicationUser>
    {
        public SMSUserManager(IUserStore<ApplicationUser> store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<ApplicationUser> passwordHasher, 
            IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<ApplicationUser>> logger) : 
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }

        public override async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            if(user == null || string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentException("User could not be empty");
            }

            var uStore = Store as SMSUserStore<ApplicationUser>;
            return await uStore.GetUserRolesAsync(user.Id);
        }

        public override async Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
        {
            var result = await base.AddPasswordAsync(user, password);
            
            if (result.Succeeded)
            {
                var uStore = Store as SMSUserStore<ApplicationUser>;
                user.LastPasswordChangedDate = DateTime.Now;

                uStore.Context.SaveChanges();
            }

            return result;

        }
        public override async Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {  
            var result =  await base.ChangePasswordAsync(user, currentPassword, newPassword);

            if (result.Succeeded)
            {
                var uStore = Store as SMSUserStore<ApplicationUser>;
                user.LastPasswordChangedDate = DateTime.Now;
                
                uStore.Context.SaveChanges();
            }

            return result;
        }

        public Company GetCompany(int companyId)
        {
            if (companyId < 0)
            {
                throw new ArgumentException("Company Id is wrong");
            }

            var uStore = Store as SMSUserStore<ApplicationUser>;
            return uStore.GetCompany(companyId);
        }

        public async Task<Company> GetCompanyAsync(int companyId)
        {
            if (companyId < 0)
            {
                throw new ArgumentException("Company Id is wrong");
            }

            var uStore = Store as SMSUserStore<ApplicationUser>;
            return await uStore.GetCompanyAsync(companyId);
        }

        public IEnumerable<CompanyClaims> GetCompanyClaims(int companyId)
        {
            if(companyId < 0)
            {
                throw new ArgumentException("Company Id is wrong");
            }

            var uStore = Store as SMSUserStore<ApplicationUser>;
            return uStore.GetCompanyClaims(companyId);
        }


        public async Task<IEnumerable<CompanyClaims>> GetCompanyClaimsAsync(int companyId)
        {
            if (companyId < 0)
            {
                throw new ArgumentException("Company Id is wrong");
            }

            var uStore = Store as SMSUserStore<ApplicationUser>;
            return await uStore.GetCompanyClaimsAsync(companyId);
        }


    }
}
