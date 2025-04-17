using Nett.Core.Domain;

namespace Nett.Core.UnitTest.Domain;

[ExcludeFromCodeCoverage]
public class EntityTest
{
    [Fact]
    public void Constructor_CreateNewGuid()
    {
        //Arrange && Act
        var entity = new UserTest();

        //Assert
        Assert.NotEqual(entity.Id, Guid.Empty);
    }
}

class UserTest : Entity { }