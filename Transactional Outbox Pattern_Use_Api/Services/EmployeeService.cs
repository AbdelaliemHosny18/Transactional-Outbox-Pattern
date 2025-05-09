using System.Text.Json;
using Transactional_Outbox_Pattern_Use_Api.Data.Entities;
using Transactional_Outbox_Pattern_Use_Api.Data;
using Transactional_Outbox_Pattern_Use_Api.IntegrationEvents;
using Transactional_Outbox_Pattern_Use_Api.Dto;

namespace Transactional_Outbox_Pattern_Use_Api.Services
{
    public class EmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public EmployeeService(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task AddEmployee(EmployeeCreatedDto employeeDto)
        {
            Employee employee = new Employee(Guid.NewGuid(),
                employeeDto.Name, employeeDto.Email);


            OutboxMessage outboxMessage = new OutboxMessage(Guid.NewGuid(),
                DateTime.Now, nameof(EmployeeCreatedEvent),
                JsonSerializer.Serialize(new EmployeeCreatedEvent { Name = employee.Name, Email = employee.Email }));
            _context.Add(employee);
            _context.Add(outboxMessage);


            await _context.SaveChangesAsync();


        }
    }
}
