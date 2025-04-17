namespace Nett.Core.Specifications;

public interface ISpecification<in T>
{
    bool IsSatisfiedBy(T instance);
}