using RMDWEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMDWEB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext() : base() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled=true;
        }
        public DbSet<ActivityLog> ActivityLog { get; set; }

        public DbSet<StatusTbl> StatusTbl { get; set; }
        public DbSet<BankTbl> BankTbl { get; set; }
        public DbSet<CurrencyTbl> CurrencyTbl { get; set; }
        public DbSet<DepartmentTbl> DepartmentTbl { get; set; }
        public DbSet<FTTComment> FTTComment { get; set; }
        public DbSet<FTTCommentLog> FTTCommentLog { get; set; }
        public DbSet<FTTDocumentfile> FTTDocumentfile { get; set; }
        public DbSet<FTTDocumentfileLog> FTTDocumentfileLog { get; set; }
        public DbSet<FTTTransaction> FTTTransaction { get; set; }
        public DbSet<FttTransactionLog> FttTransactionLog { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if(!builder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                builder.UseLazyLoadingProxies();
                builder.UseSqlServer(connectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRole");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("UserRoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }
    }
}