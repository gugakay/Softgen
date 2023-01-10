using DataAccess.Dtos.Group;
using DataAccess.Dtos.Student;
using Softgen.Infrastructure.Enums;
using Softgen.Infrastructure.Models;

namespace Softgen.Infrastructure.Extensions
{
    public static class GroupFilterExtension
    {
        public static IEnumerable<FilterQuery> ToFilterQuery(this GroupFilterDto gf)
        {
            var filters = new List<FilterQuery>();

            if (gf.Number != null && gf.Number != 0)
                filters.Add(new FilterQuery
                {
                    Field = nameof(StudentFilterDto.Name),
                    Operator = OperatorEnum.Equals,
                    Value = gf.Number.ToString()
                });

            return filters;
        }
    }
}
