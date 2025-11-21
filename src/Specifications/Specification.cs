namespace Nett.Core.Specifications;

public interface ISpecification<in T>
{
    bool IsSatisfiedBy(T instance);
}

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : ISpecification<T>
{
    public bool IsSatisfiedBy(T instance) => left.IsSatisfiedBy(instance) || right.IsSatisfiedBy(instance);
}

public class NotSpecification<T>(ISpecification<T> spec) : ISpecification<T>
{
    public bool IsSatisfiedBy(T value) => !spec.IsSatisfiedBy(value);
}

public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : ISpecification<T>
{
    public bool IsSatisfiedBy(T value) => left.IsSatisfiedBy(value) && right.IsSatisfiedBy(value);
}
