using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Teachers")]
    public class TeacherEntity
    {
        public TeacherEntity()
        {
            this.Groups = new HashSet<GroupEntity>();
        }
        [Key][Required] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string LastName { get; set; }

        [Required]
        [StringLength(11)]
        public string PrivateNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [Required] public DateTime BirthDate { get; set; }

        public ICollection<GroupEntity> Groups { get; set; }
    }
}
