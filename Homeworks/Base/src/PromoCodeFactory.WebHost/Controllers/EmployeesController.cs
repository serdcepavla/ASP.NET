using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// <returns></returns>
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
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

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
        /// <returns></returns>
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
            Employee createdEmployee = await _employeeRepository.CreateAsync(employee);
            if (createdEmployee == null)
                return NotFound();
            
            // Возварщаем свежесозданного сотрудника (возможно, в этом нет нужды, но никто не знает)
            var employeeModel = new EmployeeResponse()
            {
                Id = createdEmployee.Id,
                Email = createdEmployee.Email,
                FullName = createdEmployee.FullName,
                AppliedPromocodesCount = createdEmployee.AppliedPromocodesCount,
                Roles = createdEmployee.Roles.Select(x => new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
            };

            return Ok(employeeModel);
        }
        /// <summary>
        /// Измененние данных сотрудника
        /// </summary>
        /// <param name="employeeForUpdate"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployeeAsync(EmployeeUpdateRequest employeeForUpdate)
        {
            // Проверяем есть ли такой сотрудник в БД
            var hasEmployee = await _employeeRepository.GetByIdAsync(employeeForUpdate.Id);
            if (hasEmployee == null)
            {
                return NotFound();
            }
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
            var updatedEmployee = await _employeeRepository.UpdateAsync(employee);

            // Возвращаем обновленного сотрудника (возможно, в этом нет нужды, но никто не знает)
            var employeeModel = new EmployeeResponse()
            {
                Id = updatedEmployee.Id,
                Email = updatedEmployee.Email,
                Roles = updatedEmployee.Roles.Select(x => new RoleItemResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = updatedEmployee.FullName,
                AppliedPromocodesCount = updatedEmployee.AppliedPromocodesCount
            };

            return Ok(employeeModel);
        }
       
        /// <summary>
        /// Удаление сотруднника по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> DeleteEmployeeByIdAsync(Guid id)
        {
            var employee = _employeeRepository.DeleteByIdAsync(id).Result;

            if (employee == null)
                return NotFound();
            
            // Возвращаем фантом удаленного сотрудника (возможно, в этом нет нужды, но никто не знает)
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

            return Ok(employeeModel);
        }
    }
}