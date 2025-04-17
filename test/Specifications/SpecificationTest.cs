using Nett.Core.Specifications;

namespace Nett.Core.UnitTest.Specifications;

[ExcludeFromCodeCoverage]
public class SpecificationTest
{
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(false, true, false)]
    [InlineData(true, false, false)]
    [InlineData(false, false, false)]
    public void And_TwoSpec_TrueOnlyBothIsSatisfied(bool isAdmin, bool isActive, bool expected)
    {
        //Arrange
        var user = new User(isAdmin, isActive);
        var spec = new IsAdminSpec().And(new IsActiveSpec());

        //Act
        var isSatisfied = spec.IsSatisfiedBy(user);

        //Assert
        Assert.Equal(expected, isSatisfied);
    }

    [Theory]
    [InlineData(true, true, true)]
    [InlineData(false, true, true)]
    [InlineData(true, false, true)]
    [InlineData(false, false, false)]
    public void Or_TwoSpec_TrueIfSomeIsSatisfied(bool isAdmin, bool isActive, bool expected)
    {
        //Arrange
        var user = new User(isAdmin, isActive);
        var spec = new IsAdminSpec().Or(new IsActiveSpec());

        //Act
        var isSatisfied = spec.IsSatisfiedBy(user);

        //Assert
        Assert.Equal(expected, isSatisfied);
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(true, false)]
    public void Or_IsAdminExpec_TrueIfIsNotSatisfied(bool isAdmin, bool expected)
    {
        //Arrange
        var user = new User(isAdmin, true);
        var spec = new IsAdminSpec().Not();

        //Act
        var isSatisfied = spec.IsSatisfiedBy(user);

        //Assert
        Assert.Equal(expected, isSatisfied);
    }

    [Theory]
    [InlineData(true, false, true)]
    [InlineData(true, true, false)]
    [InlineData(false, false, false)]
    public void Or_TwoSpec_TrueIfLeftIsNotSatisfied(bool isAdmin, bool isActive, bool expected)
    {
        //Arrange
        var user = new User(isAdmin, isActive);
        var spec = new IsAdminSpec().AndNot(new IsActiveSpec());

        //Act
        var isSatisfied = spec.IsSatisfiedBy(user);

        //Assert
        Assert.Equal(expected, isSatisfied);
    }
}

[ExcludeFromCodeCoverage]
record User(bool IsAdmin, bool IsActive);

[ExcludeFromCodeCoverage]
class IsAdminSpec : ISpecification<User>
{
    public bool IsSatisfiedBy(User user) => 
        user.IsAdmin;
}

[ExcludeFromCodeCoverage]
class IsActiveSpec : ISpecification<User>
{
    public bool IsSatisfiedBy(User user) => 
        user.IsActive;
}