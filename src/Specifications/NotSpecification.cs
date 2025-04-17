namespace Nett.Core.Specifications;

public class NotSpecification<T> : ISpecification<T>
{
    private readonly ISpecification<T> _spec;

    public NotSpecification(ISpecification<T> spec) =>
        _spec = spec;

    public bool IsSatisfiedBy(T instance) =>
        !_spec.IsSatisfiedBy(instance);
}