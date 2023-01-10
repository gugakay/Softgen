using DataAccess.Dtos.Student;
using Softgen.Infrastructure.Enums;
using Softgen.Infrastructure.Models;

namespace Softgen.Infrastructure.Extensions
{
    public static class StudentFilterExtension
    {
        public static IEnumerable<FilterQuery> ToFilterQuery(this StudentFilterDto sf)
        {
            var filters = new List<FilterQuery>();

            if (!string.IsNullOrWhiteSpace(sf.Name))
                filters.Add(new FilterQuery
                {
                    Field = nameof(StudentFilterDto.Name),
                    Operator = OperatorEnum.Equals,
                    Value = sf.Name.ToLower()
                });

            if (!string.IsNullOrWhiteSpace(sf.LastName))
                filters.Add(new FilterQuery
                {
                    Field = nameof(StudentFilterDto.LastName),
                    Operator = OperatorEnum.Equals,
                    Value = sf.LastName.ToLower()
                });
            
            if (sf.BirthDate != null)
                filters.Add(new FilterQuery
                {
                    Field = nameof(StudentFilterDto.BirthDate),
                    Operator = OperatorEnum.Equals,
                    Value = sf.BirthDate.ToString() ?? string.Empty
                });

            if (!string.IsNullOrWhiteSpace(sf.PrivateNumber))
                filters.Add(new FilterQuery
                {
                    Field = nameof(StudentFilterDto.PrivateNumber),
                    Operator = OperatorEnum.Equals,
                    Value = sf.PrivateNumber.ToLower()
                });

            return filters;
        }
    }
}
