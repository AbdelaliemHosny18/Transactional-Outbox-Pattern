using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Transactional_Outbox_Pattern_Use_Api.Data;
using Transactional_Outbox_Pattern_Use_Api.Data.Entities;
using Transactional_Outbox_Pattern_Use_Api.Dto;
using Transactional_Outbox_Pattern_Use_Api.IntegrationEvents;

namespace Transactional_Outbox_Pattern_Use_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext   _dbContext;

        public EmployeesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmp([FromBody] EmployeeCreatedDto emp)
        {
            if (emp == null)
            {
                return BadRequest("Emp data is invalid.");
            }

            var new_emp= new Employee
            (
              Guid.NewGuid() ,emp.Name, emp.Email
            );
            var outboxMessage = new OutboxMessage
            (
                Guid.NewGuid(),
                DateTime.Now,
                nameof(EmployeeCreatedEvent),
                JsonSerializer.Serialize(new EmployeeCreatedEvent { Name = new_emp.Name, Email = new_emp.Email })
            );
            _dbContext.Add(outboxMessage);
            await _dbContext.Employees.AddAsync(new_emp);
            await _dbContext.SaveChangesAsync();

            return Ok();   
        }
    }
}
