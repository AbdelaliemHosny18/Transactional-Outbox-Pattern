namespace Transactional_Outbox_Pattern_Use_Api.Data.Entities
{
    public class Employee
    {
        public Employee(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
    }
}
