using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories;
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
        EfRepository<PromoCode> _promoCodeRepository;

        public PromocodesController(IRepository<PromoCode> promoCodeRepository)
        {
            _promoCodeRepository = promoCodeRepository as EfRepository<PromoCode>;
        }

        /// <summary>
        /// Получить все промокоды
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeShortResponse>>> GetPromocodesAsync()
        {
            //TODO: Получить все промокоды 
            
            var promos = await _promoCodeRepository.GetAllAsync();
            var promosModelList = promos.Select(p => new PromoCodeShortResponse
            {
                Id = p.Id,
                Code = p.Code,
                ServiceInfo = p.ServiceInfo,

                BeginDate = p.BeginDate.ToLocalTime().ToString(),
                EndDate = p.EndDate.ToLocalTime().ToString(),

                PartnerName = p.PartnerName
            }).ToList();

            return promosModelList;
        }

        /// <summary>
        /// Создать промокод и выдать его клиентам с указанным предпочтением
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(GivePromoCodeRequest request)
        {
            //TODO: Создать промокод и выдать его клиентам с указанным предпочтением

            if (request == null) //|| true
                return BadRequest(new Exception("A non-empty request body is required."));

            // Находим предпочтения
            Preference pref = _promoCodeRepository.FindAllWhere<Preference>(p => p.Name == request.Preference) 
                .FirstOrDefault();

            if (pref == null)
            {
                var allPref = _promoCodeRepository.FindAllWhere<Preference>(p => true).Select(p => p.Name).ToArray();
                var exMsg = $"Preference <{request?.Preference}> not found.\r\n" +
                            $"Pass one from list: [{string.Join(", ", allPref)}].";

                return BadRequest(new Exception(exMsg));
            }

            // Находим клиента
            var cust = _promoCodeRepository.FindAllWhere<Customer>(c => c.Preferences
                .Any(p => p.Name == request.Preference))
                .FirstOrDefault();

            if (cust == null)
                return BadRequest(new Exception($"No customer with preference <{pref?.Name}>"));

            var emp = _promoCodeRepository.FindAllWhere<Employee>(e => e.Role.Name == "Admin").FirstOrDefault();

            // Создаём промокод
            var newPromo = new PromoCode()
            {
                Code = request.PromoCode,
                PartnerName = request.PartnerName,
                ServiceInfo = request.ServiceInfo,
                Preference = pref,
                BeginDate = DateTime.Now,
                CustomerId = cust.Id,
                PartnerManager = emp
            };

            await _promoCodeRepository.CreateAsync(newPromo);
            await _promoCodeRepository.SaveChangesAsync();

            return Ok();

        }
    }
}