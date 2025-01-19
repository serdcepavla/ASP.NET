using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Context
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Customer> PromoCodes { get; set; }
        public DbSet<CustomerPreference> CustomerPreferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Roles
            modelBuilder.Entity<Role>().HasData(FakeDataFactory.Roles);

            //Employee
            modelBuilder.Entity<Employee>().HasData(FakeDataFactory.Employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.Email,
                RoleId = e.Role.Id,
                e.AppliedPromocodesCount
            }));

            //PromoCodes
            modelBuilder.Entity<PromoCode>().HasData(FakeDataFactory.PromoCodes.Select(p => new
            {
                p.Id,
                p.Code,
                p.ServiceInfo,
                p.BeginDate,
                p.EndDate,
                p.PartnerName,
                PartnerManagerId = p.PartnerManager.Id,
                PreferenceId = p.Preference.Id,
                p.CustomerId
            }));

            //CustomerPreference
            var custPrefList = FakeDataFactory.Customers
                .SelectMany(c => c.Preferences,
                 (c, p) => new { CustomerId = c.Id, PreferenceId = p.Id });

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Preferences)
                .WithMany(p => p.Customers)
                .UsingEntity<CustomerPreference>(j => j.HasData(custPrefList));

            modelBuilder.Entity<CustomerPreference>().Ignore(c => c.Id);


        }
    }
}
