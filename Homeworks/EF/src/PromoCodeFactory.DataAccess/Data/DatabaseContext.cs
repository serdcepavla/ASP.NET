using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data;

public class DatabaseContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
	public DbSet<Role> Roles { get; set; }
	public DbSet<Customer> Customers { get; set; }
	public DbSet<Preference> Preferences { get; set; }
	public DbSet<Customer> PromoCodes { get; set; }
	public DbSet<CustomerPreference> CustomerPreferences { get; set; }
	
    public DatabaseContext(DbContextOptions<DatabaseContext> options) 
        : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		//Настройка связей
		base.OnModelCreating(modelBuilder);
		
		// Employee
		modelBuilder.Entity<Employee>(employee =>
		{
			employee
				.HasOne(e => e.Role)
				.WithMany(r => r.Employees)
				.HasForeignKey(e => e.RoleId);

			employee.Property(r => r.FirstName).HasMaxLength(50);
			employee.Property(r => r.LastName).HasMaxLength(50);
			employee.Property(r => r.Email).HasMaxLength(50);
			employee.HasData(FakeDataFactory.Employees);
		});

		// Roles
		modelBuilder.Entity<Role>(role => 
		{
			role.Property(r => r.Name).HasMaxLength(50);
			role.Property(r => r.Description).HasMaxLength(250);
			role.HasData(FakeDataFactory.Roles);
		});
		
		// Customer
		modelBuilder.Entity<Customer>(customer =>
		{
			customer
				.HasMany(c => c.PromoCodes)
				.WithOne(p => p.Customer)
				.IsRequired();

			customer.Property(c => c.FirstName).HasMaxLength(50);
			customer.Property(c => c.LastName).HasMaxLength(50);
			customer.Property(c => c.Email).HasMaxLength(50);
			customer.HasData(FakeDataFactory.Customers);
		});
			
		// PromoCodes
		modelBuilder.Entity<PromoCode>(promoCode =>
		{
			promoCode.HasOne(p => p.Preference);
			promoCode.Property(p => p.Code).HasMaxLength(50);
			promoCode.Property(p => p.PartnerName).HasMaxLength(50);
			promoCode.Property(p => p.ServiceInfo).HasMaxLength(250);
			promoCode.HasData(FakeDataFactory.PromoCodes);
		});
		modelBuilder.Entity<PromoCode>()
			.HasOne(c => c.PartnerManager);

		// Preferences
		modelBuilder.Entity<Preference>(preference =>
		{
			preference.Property(p => p.Name).HasMaxLength(50);
			preference.HasData(FakeDataFactory.Preferences);
		});

		//CustomerPreference (многие ко многим)
		modelBuilder.Entity<CustomerPreference>()
			.HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

		modelBuilder.Entity<CustomerPreference>()
			.HasOne(cp => cp.Customer)
			.WithMany(c => c.CustomerPreferences)
			.HasForeignKey(cp => cp.CustomerId);

		modelBuilder.Entity<CustomerPreference>()
			.HasOne(cp => cp.Preference)
			.WithMany(p => p.CustomerPreferences)
			.HasForeignKey(cp => cp.PreferenceId);

		modelBuilder.Entity<CustomerPreference>()
			.HasData(FakeDataFactory.CustomerPreferences);
	}
}
