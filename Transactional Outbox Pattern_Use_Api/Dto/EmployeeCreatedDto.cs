using System.ComponentModel.DataAnnotations;

namespace Transactional_Outbox_Pattern_Use_Api.Dto
{
    public class EmployeeCreatedDto
    {
        [Required] public string Name { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
    }
}
