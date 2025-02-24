using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Промокоды
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
        : ControllerBase
    {
        private readonly IRepository<PromoCode> _promoCodeRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public PromocodesController(IRepository<PromoCode> promoCodeRepository, 
            IRepository<Preference> preferenceRepository,
            IRepository<Employee> employeeRepository)
        {
            _promoCodeRepository = promoCodeRepository;
            _preferenceRepository = preferenceRepository;
            _employeeRepository = employeeRepository;
        }
        
        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            //TODO: Получить все промокоды 
            var promoCodes = await _promoCodeRepository.GetAllAsync();
            
            var promoCodeModelList = promoCodes.Select(p =>
                new PromoCodeShortResponse()
                {
                    Id = p.Id,
                    Code = p.Code,
                    BeginDate = p.BeginDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    EndDate = p.EndDate.ToString("dd-MM-yyyy HH:mm:ss"),
                    PartnerName = p.PartnerName,
                    ServiceInfo = p.ServiceInfo
                }).ToList();

            return promoCodeModelList;
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением
            // Находим предпочтение
            List<Preference> preferences = _preferenceRepository.GetAllAsync().Result.ToList();

            if (preferences == null)
                return BadRequest(new Exception($"Предпочтение \"{request.Preference}\" не найдено"));
            
            // Находим клиента с указанным предпочтением 
            // (по условиям, заданным для схемы БД, промокод может быть выдан только одному клиенту)
            CustomerPreference custprefs = preferences.FirstOrDefault(x => x.Name == request.Preference)
                .CustomerPreferences.FirstOrDefault();
            
            if (custprefs == null)
                return BadRequest(new Exception($"Предпочтение \"{request.Preference}\" не присвоено ни одному клиенту"));

            // Логика задания PartnerManager не обозначена в ДЗ, поэтому упрощена
            Guid PartnerManagerId = _employeeRepository.GetAllAsync().Result.FirstOrDefault().Id;

            // Создаем промокод с указанием клиента
            PromoCode promoCode = new PromoCode()
            {
                Code = request.PromoCode,
                PartnerName = request.PartnerName,
                ServiceInfo = request.ServiceInfo,
                BeginDate = DateTime.Now.AddDays(-1),
                EndDate = DateTime.Today.AddDays(10),
                Customer = custprefs.Customer,
                Preference = custprefs.Preference,
                PartnerManagerId = PartnerManagerId
            };
            
            return Ok(await _promoCodeRepository.CreateAsync(promoCode));
        }
    }
}