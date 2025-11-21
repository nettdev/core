using Nett.Core.Domain;
using Nett.Core.Result;
using Nett.Core.Validations;

namespace Nett.Core.UnitTest.Domain;

[ExcludeFromCodeCoverage]
public class EntityTest
{
    [Fact]
    public void Constructor_CreateNewGuid()
    {
        var entity = UserTest.Create("Jose", "Joao");

        Assert.NotEqual(entity.Value!.Id, Guid.Empty);
    }

    [Fact]
    public void Create_WithValidValues_ShouldReturnSucess()
    {
        var result = UserTest.Create("Jose", "Joao");

        Assert.NotNull(result.Value);
    }

    [Theory]
    [InlineData("", "Joao", false, "FirstName cannot be empty")]
    [InlineData("Jose", "", false, "LastName cannot be empty")]
    public void Create_WithInvalidValues_ReturnsError(string firstName, string lastName, bool isSuccess, string errorMessage)
    {
        var result = UserTest.Create(firstName, lastName);

        Assert.Equal(result.IsSuccess, isSuccess);
        Assert.Equal(result.Error!.InnerErrors[0].Message, errorMessage);
    }

    private class UserTest : Entity
    {
        public string FistName { get; private set; }
        public string LastName { get; private set; }

        public static Result<UserTest, Error> Create(string? firstName, string? lastName)
        {
            return ParameterRuleBuilder
                .RuleFor(() => firstName).NotEmpty()
                .RuleFor(() => lastName).NotEmpty()
                .Build(() => new UserTest(){ FistName = firstName!, LastName = lastName! });
        }

        #nullable disable
        private UserTest()
        { }
    }
}
