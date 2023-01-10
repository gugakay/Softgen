using System.ComponentModel.DataAnnotations;

namespace DataAccess.Dtos.Teacher;

public class TeacherUpdateDto
{
    [Required] public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    [StringLength(11)] public string PrivateNumber { get; set; }
    [EmailAddress] public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
}
