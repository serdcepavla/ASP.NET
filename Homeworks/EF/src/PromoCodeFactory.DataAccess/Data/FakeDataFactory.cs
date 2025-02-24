using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.DataAccess.Data
{
    public static class FakeDataFactory
    {
        public static IEnumerable<Employee> Employees => new List<Employee>()
        {
            new Employee()
            {
                Id = Guid.Parse("451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"),
                Email = "owner@somemail.ru",
                FirstName = "Иван",
                LastName = "Сергеев",
                RoleId = Roles.FirstOrDefault(x => x.Name == "Admin").Id,
                AppliedPromocodesCount = 5
            },
            new Employee()
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                RoleId = Roles.FirstOrDefault(x => x.Name == "PartnerManager").Id,
                AppliedPromocodesCount = 10
            },
        };

        public static IEnumerable<Role> Roles => new List<Role>()
        {
            new Role()
            {
                Id = Guid.Parse("53729686-a368-4eeb-8bfa-cc69b6050d02"),
                Name = "Admin",
                Description = "Администратор",
            },
            new Role()
            {
                Id = Guid.Parse("b0ae7aac-5493-45cd-ad16-87426a5e7665"),
                Name = "PartnerManager",
                Description = "Партнерский менеджер"
            }
        };

        public static IEnumerable<Preference> Preferences => new List<Preference>()
        {
            new Preference()
            {
                Id = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c"),
                Name = "Театр",
            },
            new Preference()
            {
                Id = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd"),
                Name = "Семья",
            },
            new Preference()
            {
                Id = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84"),
                Name = "Дети",
            }
        };

        public static IEnumerable<Customer> Customers => new List<Customer>()
        {
            new Customer()
            {
                Id = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
                Email = "ivan_sergeev@mail.ru",
                FirstName = "Иван",
                LastName = "Петров"
                //TODO: Добавить предзаполненный список предпочтений
                // Добавлено ниже
            },
            new Customer()
            {
                Id = Guid.Parse("D54F3EFF-5B25-42F2-BDEA-067EC5130DC5"),
                Email = "niko_sidorov@ya.ru",
                FirstName = "Николай",
                LastName = "Сидоров"
                //TODO: Добавить предзаполненный список предпочтений
                // Добавлено ниже
            }
        };

        public static IEnumerable<CustomerPreference> CustomerPreferences => new List<CustomerPreference>()
        {
            new CustomerPreference()
            {
                CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
                PreferenceId = Guid.Parse("ef7f299f-92d7-459f-896e-078ed53ef99c")
            },
            new CustomerPreference()
            {
                CustomerId = Guid.Parse("D54F3EFF-5B25-42F2-BDEA-067EC5130DC5"),
                PreferenceId = Guid.Parse("c4bda62e-fc74-4256-a956-4760b3858cbd")
            },
            new CustomerPreference()
            {
                CustomerId = Guid.Parse("D54F3EFF-5B25-42F2-BDEA-067EC5130DC5"),
                PreferenceId = Guid.Parse("76324c47-68d2-472d-abb8-33cfa8cc0c84")
            }
        };

        public static  IEnumerable<PromoCode> PromoCodes => new List<PromoCode>()
        {
            new PromoCode()
            {
                Id = Guid.Parse("321bbf73-c525-47b7-a615-b7ed600c70b7"),
                Code = "PC-1",
                ServiceInfo = $"Промокод на {(new Random()).Next(1, 99)}%",
                BeginDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Today.AddDays(10),
                PartnerName = "OZON",
                PartnerManagerId = Employees.ToArray()[0].Id,
                PreferenceId = Preferences.ToArray()[0].Id,
                CustomerId = Customers.ToArray()[0].Id
            },
            new PromoCode()
            {
                Id = Guid.Parse("f3bb4250-b000-4c52-9238-9558d5820eca"),
                Code = "PC-2",
                ServiceInfo = $"Промокод на {(new Random()).Next(1, 99)}%",
                BeginDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Today.AddDays(10),
                PartnerName = "Wildberries",
                PartnerManagerId = Employees.ToArray()[1].Id,
                PreferenceId = Preferences.ToArray()[1].Id,
                CustomerId = Customers.ToArray()[1].Id
            }            
        };
    }
}