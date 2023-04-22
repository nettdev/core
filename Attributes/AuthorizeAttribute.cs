namespace Nett.Core;

public class AuthorizeAttribute : Attribute
{
    public string Name { get; }
    public string Group { get; }
    public string Description { get; }

    public AuthorizeAttribute(string name, string group, string description)
    {
        Name = name;
        Group = group;
        Description = description;
    }
}