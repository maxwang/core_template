using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<CompanyClaims> CompanyClaims { get; set; }

        public DbSet<ApplicationRoleType> RoleTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(au =>
            {
                au.HasOne(c => c.MyCompany).WithMany(cu => cu.Users).HasForeignKey(ccu => ccu.CompanyId);
                au.Property(c => c.CompanyId).IsRequired(false);

                au.Property(c => c.CreationDate).IsRequired(true).HasDefaultValueSql("getdate()");
                au.Property(c => c.LastPasswordChangedDate).IsRequired(true).HasDefaultValueSql("getdate()");

            });

            builder.Entity<ApplicationRole>(ar =>
            {
                ar.HasOne(a => a.RoleType).WithMany(at => at.Roles).HasForeignKey(atr => atr.RoleTypeId);
                ar.Property(c => c.IsInternal).IsRequired(true).HasDefaultValue(false);
            });


            builder.Entity<Company>(c =>
            {
                c.HasIndex(u => u.CreatedTime).HasName("CreationTimeIndex");
                c.Property(u => u.CreatedTime).HasDefaultValueSql("getdate()");
                //c.HasMany(au => au.Users).WithOne(auc => auc.MyCompany).HasForeignKey(aucc => aucc.CompanyId).IsRequired();
                c.HasMany(au => au.Claims).WithOne(auc => auc.Company).HasForeignKey(aucc => aucc.CompanyId).IsRequired();
            });

            builder.Entity<CompanyClaims>(cc =>
            {
                cc.HasOne(ccc => ccc.Company).WithMany(cccc => cccc.Claims).HasForeignKey(aucc => aucc.CompanyId).IsRequired();
            });
        }
    }
}
