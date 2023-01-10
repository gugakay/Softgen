using Softgen.Infrastructure.Enums;

namespace Softgen.Infrastructure.Models;

public class FilterQuery
{
    public string Field { get; set; }
    public OperatorEnum Operator { get; set; }
    public string Value { get; set; }
    public IEnumerable<string> ValueListed { get; set; }
}
