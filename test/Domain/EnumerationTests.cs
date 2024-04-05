namespace Nett.Core.UnitTest;

public class EnumerationTest
{
    [Fact]
    public void Equal_SameValue_ReturnTrue()
    {
        //Arrange && Act
        var editor1 = UserType.Editor;
        var editor2 = UserType.Editor;
    
        //Assert
        Assert.Equal(editor1, editor2);
    }

    [Fact]
    public void ToString_ReturnEnumName()
    {
        //Arrange && Act
        var editor = UserType.Editor;
    
        //Assert
        Assert.Equal("Editor", editor?.ToString());
    }
}

class UserType(string name, int value) : Enumeration<UserType>(name, value)
{
    public static UserType Editor = new (nameof(Editor), 0);
    public static UserType Admin = new (nameof(Admin), 1);
}