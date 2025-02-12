using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly EfRepository<Customer> _customerRepository;
        IRepository<Preference> _preferenceRepository;
        IRepository<CustomerPreference> _cpRepository;


        // public CustomersController(EfRepository<Customer> customerRepository)
        // {
        //     _customerRepository = customerRepository;
        // }

        public CustomersController(IEnumerable<IRepository<Customer>> custRepos, IRepository<Preference> prefRepo, IRepository<CustomerPreference> cpRepo) 
        {
            _customerRepository = custRepos.OfType<EfRepository<Customer>>().SingleOrDefault();

            if (_customerRepository == null) 
                throw new Exception("Service EfRepository<Customer> hasn't been injected to container");

            _preferenceRepository = prefRepo;
            _cpRepository = cpRepo;

        }


        /// <summary>
        /// Получение списка всех клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            //TODO: Добавить получение списка клиентов
            //throw new NotImplementedException();
            var customers = await _customerRepository.GetAllAsync();

            var customersModelList = customers.Select(x =>
                new CustomerShortResponse()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email
                }).ToList();

            return Ok(customersModelList);
        }
        /// <summary>
        /// Получение клиента вместе с выданными ему промомкодами
        /// </summary>
        /// <param name="id">GUID клиента</param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            //TODO: Добавить получение клиента вместе с выданными ему промомкодами
            var customer = await _customerRepository.GetByIdAsync(x => x.Id == id, ["Preferences", "PromoCodes"]);
            
            if (customer == null)
                return NotFound();

            var customerModel = new CustomerResponse()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Preferences = customer.Preferences.Select(p => p.Name).ToList(),
                PromoCodes = customer.PromoCodes.Select(p => new PromoCodeShortResponse
                {
                    Id = p.Id,
                    Code = p.Code,
                    ServiceInfo = p.ServiceInfo,
                    BeginDate = p.BeginDate.ToUniversalTime().ToString(),
                    EndDate = p.EndDate.ToUniversalTime().ToString(),
                    PartnerName = p.PartnerName
                }).ToList()
            };

            return customerModel;

        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            //TODO: Добавить создание нового клиента вместе с его предпочтениями
            var newCustomer = new Customer()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email                
            };

            // Добавляем клиента в репозиторий
            await _customerRepository.CreateAsync(newCustomer);           
            await _customerRepository.SaveChangesAsync();

            // Получаем предпочтения клиента
            List<Preference> prefs = (await _preferenceRepository.GetAllAsync())
                .Where(p => request.PreferenceIds.Contains(p.Id)).ToList();

            // Добавляем связи клиента с предпочтениями
            var custPrefList = prefs.Select(p => new CustomerPreference { CustomerId = newCustomer.Id, PreferenceId = p.Id });

            foreach (var cp in custPrefList)
                await _cpRepository.CreateAsync(cp);

            await _cpRepository.SaveChangesAsync(); 

            newCustomer.Preferences = prefs;

            var result = CreatedAtAction(nameof(GetCustomerAsync),
                new { newCustomer.Id },
                newCustomer);
            
            return result; 
        }

        /// <summary>
        /// Редактирование данных клиента вместе с его предпочтениями
        /// </summary>
        /// <param name="id">GUID клиента</param>
        /// <param name="request">Данные для обновления</param>
        /// <returns>Ок</returns>

        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            //TODO: Обновить данные клиента вместе с его предпочтениями
            var cust = await _customerRepository.GetByIdAsync(c => c.Id == id, ["Preferences"]); 

            if (cust == null) 
                return NotFound();

            // Обновляем данные клиента
            cust.FirstName = request.FirstName;
            cust.LastName = request.LastName; 
            cust.Email = request.Email;
            
            // Удаляем старые предпочтения клиента
            cust.Preferences.Clear();

            // Сохраняем
            await _customerRepository.SaveChangesAsync(); 

            // Получаем предпочтения из БД
            List<Preference> prefs = (await _preferenceRepository.GetAllAsync())
                .Where(p => request.PreferenceIds.Contains(p.Id)).ToList();

            // Создаем связи клиента с предпочтениями
            var custPrefList = prefs.Select(p => new CustomerPreference { CustomerId = cust.Id, PreferenceId = p.Id });
            
            foreach (var cp in custPrefList)
                await _cpRepository.CreateAsync(cp);

            // Сохраняем
            await _cpRepository.SaveChangesAsync(); 

            return Ok();
        }

        /// <summary>
        /// Удаление клиента вместе с выданными ему промокодами
        /// </summary>
        /// <param name="id">GUID клиента</param>
        /// <returns>Ок / Не найден</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            //TODO: Удаление клиента вместе с выданными ему промокодами
            var isDeleted = await _customerRepository.DeleteByIdAsync(id);
            if (isDeleted) _customerRepository.SaveChanges();

            return (isDeleted)? Ok() : NotFound();
        }
    }
}