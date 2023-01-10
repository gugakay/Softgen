using System.Linq.Expressions;

namespace Softgen.Infrastructure.Option.Expressions;

internal sealed class IdentitySpecification<T> : BaseFilterSpecification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        return x => true;
    }
}
public abstract class BaseFilterSpecification<T>
{
    public static readonly BaseFilterSpecification<T> All = new IdentitySpecification<T>();

    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();

        return predicate(entity);
    }
    public abstract Expression<Func<T, bool>> ToExpression();
    public BaseFilterSpecification<T> And(BaseFilterSpecification<T> specification)
    {
        if (this == All)
            return specification;

        if (specification == All)
            return this;

        return new AndSpecification<T>(this, specification);
    }
    public BaseFilterSpecification<T> Or(BaseFilterSpecification<T> specification)
    {
        if (this == All || specification == All)
            return All;

        return new OrSpecification<T>(this, specification);
    }
    public BaseFilterSpecification<T> Not()
    {
        return new NotSpecification<T>(this);
    }
}
internal sealed class AndSpecification<T> : BaseFilterSpecification<T>
{
    private readonly BaseFilterSpecification<T> _left;
    private readonly BaseFilterSpecification<T> _right;

    public AndSpecification(BaseFilterSpecification<T> left, BaseFilterSpecification<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        BinaryExpression andExpression = Expression.AndAlso(leftExpression.Body, rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(andExpression, leftExpression.Parameters.Single());
    }
}
internal sealed class OrSpecification<T> : BaseFilterSpecification<T>
{
    private readonly BaseFilterSpecification<T> _left;
    private readonly BaseFilterSpecification<T> _right;

    public OrSpecification(BaseFilterSpecification<T> left, BaseFilterSpecification<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> leftExpression = _left.ToExpression();
        Expression<Func<T, bool>> rightExpression = _right.ToExpression();

        BinaryExpression orExpression = Expression.OrElse(leftExpression.Body, rightExpression.Body);

        return Expression.Lambda<Func<T, bool>>(orExpression, leftExpression.Parameters.Single());
    }
}
internal sealed class NotSpecification<T> : BaseFilterSpecification<T>
{
    private readonly BaseFilterSpecification<T> _specification;

    public NotSpecification(BaseFilterSpecification<T> specification)
    {
        _specification = specification;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = _specification.ToExpression();

        UnaryExpression notExpression = Expression.Not(expression.Body);

        return Expression.Lambda<Func<T, bool>>(notExpression, expression.Parameters.Single());
    }
}
