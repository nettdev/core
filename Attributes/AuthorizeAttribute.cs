namespace Nett.Core;

public class ResourceAttribute : Attribute
{
    public string Name { get; }
    public string Group { get; }
    public string Description { get; }

    public ResourceAttribute(string name, string group, string description)
    {
        Name = name;
        Group = group;
        Description = description;
    }
}