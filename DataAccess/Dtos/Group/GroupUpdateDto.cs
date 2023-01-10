using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DataAccess.Dtos.Group;

public class GroupUpdateDto
{
    [Required] public int Id { get; set; }
    public string Name { get; set; }
    [Range(1, 1000)]
    public int? Number { get; set; }
}
