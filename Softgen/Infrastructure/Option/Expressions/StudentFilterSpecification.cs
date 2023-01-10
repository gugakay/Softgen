using DataAccess.Dtos.Student;
using DataAccess.Entities;
using Softgen.Infrastructure.Enums;
using Softgen.Infrastructure.Models;
using System.Linq.Expressions;

namespace Softgen.Infrastructure.Option.Expressions
{
    public class StudentFilterSpecification : BaseFilterSpecification<StudentEntity>
    {
        private Expression<Func<StudentEntity, bool>> BaseExpression { get; set; } = b => true;
        private readonly IEnumerable<FilterQuery> _filters;

        public StudentFilterSpecification(IEnumerable<FilterQuery> filters)
        {
            _filters = filters;
        }

        public override Expression<Func<StudentEntity, bool>> ToExpression()
        {
            var result = BaseExpression;

            if (_filters == null) return result;

            foreach (var item in _filters)
            {
                if (string.IsNullOrWhiteSpace(item.Value) && (item.ValueListed == null || !item.ValueListed.Any()))
                    continue;

                switch (item.Field)
                {
                    case nameof(StudentFilterDto.Name):
                        {
                            switch (item.Operator)
                            {
                                case OperatorEnum.Equals:
                                    result = result.And(x => x.Name.ToLower().Equals(item.Value)); break;
                            }

                        }
                        break;

                    case nameof(StudentFilterDto.LastName):
                        {
                            switch (item.Operator)
                            {
                                case OperatorEnum.Equals:
                                    result = result.And(x => x.Name.ToLower().Equals(item.Value)); break;
                            }

                        }
                        break;

                    case nameof(StudentFilterDto.BirthDate):
                        {
                            switch (item.Operator)
                            {
                                case OperatorEnum.Equals:
                                    result = result.And(x => x.BirthDate.Date == Convert.ToDateTime(item.Value).Date); break;
                            }
                        }
                        break;

                    case nameof(StudentFilterDto.PrivateNumber):
                        {
                            switch (item.Operator)
                            {
                                case OperatorEnum.Equals:
                                    result = result.And(x => x.PrivateNumber.ToLower().Equals(item.Value)); break;
                            }
                        }
                        break;
                }
            }

            return result;
        }
    }
}
