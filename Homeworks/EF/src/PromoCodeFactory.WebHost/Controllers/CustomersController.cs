using Microsoft.AspNetCore.Mvc;
using Namotion.Reflection;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
        : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository,
            IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить список всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        [HttpGet]
        public async Task<List<CustomerShortResponse>> GetCustomersAsync()
        {
            //TODO: Добавить получение списка клиентов
            var customers = await _customerRepository.GetAllAsync();
            
            var customerModelList = customers.Select(x =>
                new CustomerShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                }).ToList();

            return customerModelList;
        }

        /// <summary>
        /// Получить данные клиента (с промокодами и предпочтениями) по Id
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <returns>Данные клиента</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Добавить получение клиента вместе с выданными ему промомкодами
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null) 
                return NotFound(id);

            var response = new CustomerResponse()
            {
                Email = customer.Email,
                FirstName = customer.FirstName,
                Id = customer.Id,
                LastName = customer.LastName,
                PromoCodes = customer.PromoCodes
                    .Select(p => new PromoCodeShortResponse()
                    {
                        Id = p.Id,
                        Code = p.Code,
                        BeginDate = p.BeginDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        EndDate = p.EndDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        PartnerName = p.PartnerName,
                        ServiceInfo = p.ServiceInfo,
                    })
                    .ToList(),
                Preferences = customer.CustomerPreferences.Select(cp => new PrefernceResponse
                {
                    Id = cp.PreferenceId,
                    Name = cp.Preference.Name
                }).ToList()
            };
            return Ok(response);
        }

        /// <summary>
        /// Добавить нового клиента
        /// </summary>
        /// <param name="request">Данные нового клиента</param>
        /// <returns>Данные нового клиента</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Добавить создание нового клиента вместе с его предпочтениями
            // Создаем клиента
            var customer = new Customer()
            {
                FirstName = request.FirstName,
                LastName= request.LastName,
                Email = request.Email,
            };
            
            // Добавляем клиента в БД, чтобы он получил свой Id
            customer = await _customerRepository.CreateAsync(customer);
            
            // Добавляем предпочтения клиенту
            customer.CustomerPreferences = request.PreferenceIds
                .Select(pid => new CustomerPreference { CustomerId = customer.Id, PreferenceId = pid })
                .ToList();

            // Добавляем предпочтения в БД
            await _customerRepository.UpdateAsync(customer);

            return Ok(customer);
        }

        /// <summary>
        /// Редактировать данные клиента вместе с его предпочтениями
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <param name="request">Данные клиента для обновления</param>
        /// <returns>Данные клиента после изменения</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            var customer = await _customerRepository.GetByIdAsync(id);
            
            if (customer == null)
                return NotFound(id);

            var preferences = await _preferenceRepository.GetRangeByIdsAsync(request.PreferenceIds);

            customer.LastName = request.LastName;
            customer.FirstName = request.FirstName;
            customer.Email = request.Email;

            customer.CustomerPreferences.Clear();
            customer.CustomerPreferences = preferences.Select(x => new CustomerPreference()
            {
                Customer = customer,
                Preference = x
            }).ToList();

            await _customerRepository.UpdateAsync(customer);

            return Ok(customer);
        }

        /// <summary>
        /// Удалить данные клиента вместе с выданными ему промокодами
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <returns>Ок / Не найден</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return (await _customerRepository.DeleteAsync(customer)) ? Ok() : NotFound();
        }
    }
}