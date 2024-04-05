namespace Nett.Core;

public static class SpecificationExtensions
{
    public static ISpecification<T> And<T>(this ISpecification<T> @this, ISpecification<T> right) =>
        new AndSpecification<T>(@this, right);

    public static ISpecification<T> Or<T>(this ISpecification<T> @this, ISpecification<T> right) =>
        new OrSpecification<T>(@this, right);

    public static ISpecification<T> Not<T>(this ISpecification<T> @this) =>
        new NotSpecification<T>(@this);

    public static ISpecification<T> AndNot<T>(this ISpecification<T> @this, ISpecification<T> other) =>
        new AndSpecification<T>(@this, other.Not());
}