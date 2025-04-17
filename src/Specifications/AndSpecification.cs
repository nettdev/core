namespace Nett.Core.Specifications;

public class AndSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right) =>
        (_left, _right) = (left, right);

    public bool IsSatisfiedBy(T instance) =>
        _left.IsSatisfiedBy(instance) && _right.IsSatisfiedBy(instance);
}