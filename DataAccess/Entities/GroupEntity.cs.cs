using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Groups")]
    public class GroupEntity
    {
        public GroupEntity()
        {
            this.Students = new HashSet<StudentEntity>();
        }

        [Key][Required] public int Id { get; set; }
        [Required]public string Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public int Number { get; set; }

        public ICollection<StudentEntity> Students { get; set; }
        [ForeignKey("Teacher")] public int? TeacherId { get; set; }
        public TeacherEntity Teacher { get; set; }
    }
}
