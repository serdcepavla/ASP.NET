﻿using System;
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
                Role = Roles.FirstOrDefault(x => x.Name == "Admin"),
                AppliedPromocodesCount = 5
            },
            new Employee()
            {
                Id = Guid.Parse("f766e2bf-340a-46ea-bff3-f1700b435895"),
                Email = "andreev@somemail.ru",
                FirstName = "Петр",
                LastName = "Андреев",
                Role = Roles.FirstOrDefault(x => x.Name == "PartnerManager"),
                AppliedPromocodesCount = 10
            },
        };

        //public static IEnumerable<PromoCode> PromoCodes => new List<PromoCode>()
        //{
        //    new PromoCode() {
        //        Id = Guid.Parse("6A2A2AED-49D8-4C8F-9C9A-160AC43DAD9D"),
        //        BeginDate = DateTime.Now,
        //        Code = "12345",
        //        CustomerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"),
        //        EndDate = DateTime.Now,
        //        PartnerManager =  Employees.FirstOrDefault(e => e.Role.Name == "PartnerManager"),
        //        PartnerName = "OZON",
        //        Preference = Preferences.FirstOrDefault(),
        //        ServiceInfo = "Некий промокод"
        //    }
        //};


        public static IEnumerable<PromoCode> PromoCodes =>
            Enumerable.Range(1, 10)
            .Select(i => new PromoCode()
            {
                Id = Guid.NewGuid(),
                Code = $"PC-{i}",
                ServiceInfo = $"Промокод на {(new Random()).Next(1, 99)}%",
                BeginDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Today.AddDays(10),
                PartnerName = "OZON",
                PartnerManager = Employees.FirstOrDefault(e => e.Role.Name == "PartnerManager"),
                Preference = Preferences.ToArray()[(new Random()).Next(Preferences.Count())],
                CustomerId = Customers.ToArray()[(new Random()).Next(Customers.Count())].Id
            }
            )
            .Union(
                    Enumerable.Range(11, 15)
                    .Select(i => new PromoCode()
                    {
                        Id = Guid.NewGuid(),
                        Code = $"PC-{i}",
                        ServiceInfo = $"Промокод на {(new Random()).Next(1, 99)}%",
                        BeginDate = DateTime.Now.AddDays(-1),
                        EndDate = DateTime.Today.AddDays(10),
                        PartnerName = "Wildberries",
                        PartnerManager = Employees.FirstOrDefault(e => e.Role.Name == "Admin"),
                        Preference = Preferences.ToArray()[(new Random()).Next(Preferences.Count())],
                        CustomerId = Customers.ToArray()[(new Random()).Next(Customers.Count())].Id
                    }
                    )
                );

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

        public static IEnumerable<Customer> Customers
        {
            get
            {
                var customerId = Guid.Parse("a6c8c6b1-4349-45b0-ab31-244740aaf0f0");
                var customers = new List<Customer>()
                {
                    new Customer()
                    {
                        Id = customerId,
                        Email = "ivan_sergeev@mail.ru",
                        FirstName = "Иван",
                        LastName = "Петров",
                        //TODO: Добавить предзаполненный список предпочтений
                        Preferences =  Preferences.Where( p => Regex.IsMatch(p.Name, @"Дети|Семья")).ToList()
                    },
                    new Customer()
                    {
                        Id = Guid.Parse("D54F3EFF-5B25-42F2-BDEA-067EC5130DC5"),
                        Email = "lev_tolstoy@gmail.com",
                        FirstName = "Лев",
                        LastName = "Толстой",
                        //TODO: Добавить предзаполненный список предпочтений
                        Preferences =  Preferences.Where( p => Regex.IsMatch(p.Name, @"Бизнес|Театр")).ToList()
                    }

                };

                return customers;
            }
        }
    }
}