namespace Nett.Core;

public interface ISpecification<in T>
{
    bool IsSatisfiedBy(T instance);
}