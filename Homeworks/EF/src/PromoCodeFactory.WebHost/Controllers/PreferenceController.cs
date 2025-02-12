using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Repositories;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers;

/// <summary>
/// Предпочтения
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class PreferenceController : ControllerBase
{
    IRepository<Preference> _preferenceRepository;

    public PreferenceController(IEnumerable<IRepository<Preference>> repositories)
    {
        _preferenceRepository = repositories.OfType<EfRepository<Preference>>().SingleOrDefault(); 

        if (_preferenceRepository == null)
            throw new Exception("Service EfRepository<Preference> hasn't been injected to container");
    }

    /// <summary>
    /// Получение списка предпочтений клиентов
    /// </summary>
    /// <returns>Cписок предпочтений</returns>
    [HttpGet]
    public async Task<List<PreferenceResponse>> GetPrererencesAsync()
    {
        var prefs = await _preferenceRepository.GetAllAsync();
        var prefsModelList = prefs.Select(p => new PreferenceResponse()
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();

        return prefsModelList;
    }
}