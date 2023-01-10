using DataAccess.Dtos.Group;
using DataAccess.Entities;
using Softgen.Infrastructure.Enums;
using Softgen.Infrastructure.Models;
using System.Linq.Expressions;

namespace Softgen.Infrastructure.Option.Expressions
{
    public class GroupFilterSpecification : BaseFilterSpecification<GroupEntity>
    {
        private Expression<Func<GroupEntity, bool>> BaseExpression { get; set; } = b => true;
        private readonly IEnumerable<FilterQuery> _filters;

        public GroupFilterSpecification(IEnumerable<FilterQuery> filters)
        {
            _filters = filters;
        }

        public override Expression<Func<GroupEntity, bool>> ToExpression()
        {
            var result = BaseExpression;

            if (_filters == null) return result;

            foreach (var item in _filters)
            {
                if (string.IsNullOrWhiteSpace(item.Value) && (item.ValueListed == null || !item.ValueListed.Any()))
                    continue;

                switch (item.Field)
                {
                    case nameof(GroupFilterDto.Number):
                        {
                            switch (item.Operator)
                            {
                                case OperatorEnum.Equals:
                                    result = result.And(x => x.Number.Equals(item.Value)); break;
                            }

                        }
                        break;
                }
            }

            return result;
        }
    }
}
