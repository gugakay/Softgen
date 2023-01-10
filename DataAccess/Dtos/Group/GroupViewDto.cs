using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DataAccess.Dtos.Group
{
    public class GroupViewDto
    {
        [Required] public string Name { get; set; }
        [Required]
        [Range(1, 1000)]
        public int Number { get; set; }
    }
}
