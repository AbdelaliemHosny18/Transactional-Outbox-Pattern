namespace Transactional_Outbox_Pattern_Use_Api.IntegrationEvents
{
    public class EmployeeCreatedEvent
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
