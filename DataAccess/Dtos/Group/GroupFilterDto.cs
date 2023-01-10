using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace DataAccess.Dtos.Group
{
    public class GroupFilterDto
    {
        [Range(1, 1000)]
        public int? Number { get; set; }
    }
}
