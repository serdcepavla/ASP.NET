using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private readonly IRepository<Preference> _preferenceRepository;

        public PreferencesController(IRepository<Preference> preferenceRepository)
        {
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить список всех предпочтений
        /// </summary>
        /// <returns>Список предпочтений</returns>
        [HttpGet]
        public async Task<List<PrefernceResponse>> GetPreferencesAsync()
        {
            //TODO: Добавить получение списка клиентов
            var preferences = await _preferenceRepository.GetAllAsync();
            
            var preferenceModelList = preferences.Select(x =>
                new PrefernceResponse()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();

            return preferenceModelList;
        }
    }
}
