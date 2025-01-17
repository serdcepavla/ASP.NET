using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns>Список данных всех сотрудников</returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns>Данные сотрудника</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound("Не удалось найти сотрудника");

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="employeeForCreate"></param>
        /// <returns>Даннные добавленного сотрудника</returns>
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> CreateEmployeeAsync(EmployeeCreateRequest employeeForCreate)
        {
            // Создаем экземпляр сотрудника
            Employee employee = new()
            {
                FirstName = employeeForCreate.FirstName,
                LastName = employeeForCreate.LastName,
                Email = employeeForCreate.Email,
                AppliedPromocodesCount = employeeForCreate.AppliedPromocodesCount,
                Roles = employeeForCreate.Roles
            };

            // Сохранняем экземпляр сотрудника в БД
            bool isCreated = await Task.FromResult(_employeeRepository.CreateAsync(employee));

            if (isCreated)
                return Ok(employee);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось добавить сотрудника");
        }
        
        /// <summary>
        /// Измененние данных сотрудника
        /// </summary>
        /// <param name="employeeForUpdate"></param>
        /// <returns>Измененные данные сотрудника</returns>
        [HttpPut]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployeeAsync(EmployeeUpdateRequest employeeForUpdate)
        {
            // Проверяем есть ли такой сотрудник в БД
            var hasEmployee = await _employeeRepository.GetByIdAsync(employeeForUpdate.Id);
            
            if (hasEmployee == null)
                return NotFound("Не удалось найти сотрудника для изменения его данных");

            // Создаем экземпляр сотрудника
            Employee employee = new()
            {
                Id = employeeForUpdate.Id,
                FirstName = employeeForUpdate.FirstName,
                LastName = employeeForUpdate.LastName,
                Email = employeeForUpdate.Email,
                AppliedPromocodesCount = employeeForUpdate.AppliedPromocodesCount,
                Roles = employeeForUpdate.Roles
            };
            // Обновляем экземпляр сотрудника в БД
            if (_employeeRepository.UpdateAsync(employee))
                return Ok(employee);
            else 
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось изменить данные сотрудника");
        }

        /// <summary>
        /// Удаление сотруднника по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Результат удаления данных сотрудника</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployeeByIdAsync(Guid id)
        {
            var hasEmployee = await _employeeRepository.GetByIdAsync(id);
            
            if (hasEmployee == null)
                return NotFound("Не удалось найти сотрудника для удаления его данных");

            if (_employeeRepository.DeleteByIdAsync(id))
                return NoContent();
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось удалить данные сотрудника");
        }
    }
}