using System.ComponentModel.DataAnnotations;

namespace DataAccess.Dtos.Student;

public class StudentCreateDto
{
    [Required] public string Name { get; set; }
    [Required] public string LastName { get; set; }
    [Required][StringLength(11)] public string PrivateNumber { get; set; }
    [Required][EmailAddress] public string Email { get; set; }
    [Required] public DateTime BirthDate { get; set; }
}
