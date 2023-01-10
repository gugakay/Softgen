using System.ComponentModel.DataAnnotations;

namespace DataAccess.Dtos.Teacher
{
    public class TeacherFilterDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        [StringLength(11)] public string PrivateNumber { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
