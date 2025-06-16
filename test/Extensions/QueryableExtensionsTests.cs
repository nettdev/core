using FluentAssertions;
using Nett.Core.Extensions;
using Nett.Core.Specifications;

namespace Nett.Core.UnitTest.Extensions;

[ExcludeFromCodeCoverage]
public class QueryableExtensionsTests
{
    [Fact]
    public void CreateQuery_WithQuerySpecification_ShouldReturnFilteredList()
    {
        //Arrange
       var query = new QuerySpecificationTest();
       var people = GetPeople();
    
        //Act
        var result = people.CreateQuery(query).ToList();
    
        //Assert
        result[0].Should().Be("Bob");
    }

    [Fact]
    public void OrderByColumn_SortsByAgeDescending()
    {
        var people = GetPeople();
        var sorted = people.OrderByColumn("Age", "desc").ToList();

        Assert.Equal(35, sorted[0].Age);
        Assert.Equal(30, sorted[1].Age);
        Assert.Equal(25, sorted[2].Age);
    }

    [Fact]
    public void ThenByColumn_SortsByNameThenAge()
    {
        var people = new List<Person>
        {
            new("Alex", 40, null!),
            new("Alex", 30, null!),
            new("Ben", 25, null!)
        }.AsQueryable();

        var sorted = people
            .OrderByColumn("Name")
            .ThenByColumn("Age", "desc")
            .ToList();

        Assert.Equal(40, sorted[0].Age);  // Alex with age 40
        Assert.Equal(30, sorted[1].Age);  // Alex with age 30
        Assert.Equal("Ben", sorted[2].Name);
    }

    [Fact]
    public void OrderByColumn_SortsByNestedProperty()
    {
        var people = GetPeople();
        var sorted = people.OrderByColumn("Address.City").ToList();

        Assert.Equal("Amsterdam", sorted[0].Address.City);
        Assert.Equal("Berlin", sorted[1].Address.City);
        Assert.Equal("Zurich", sorted[2].Address.City);
    }

    [Fact]
    public void OrderByColumn_InvalidColumn_ThrowsException()
    {
        var people = GetPeople();

        Assert.Throws<ArgumentException>(() =>
        {
            var sorted = people.OrderByColumn("NonExistentProperty").ToList();
        });
    }

    private class QuerySpecificationTest : QuerySpecification<Person, string>
    {
        public QuerySpecificationTest()
        {
            Predicate = query => query.Name != "";
            OrderBy = query => query.Name;
            Take = 1;
            Skip = 1;
            Selector = query => query.Name;
        }
    } 

    private record Person(string Name, int Age, Address Address);

    private record Address(string City);

    private static IQueryable<Person> GetPeople() => new List<Person>
        {
            new("Alice", 30, new Address("Zurich")),
            new("Bob", 25, new Address("Amsterdam")),
            new("Charlie", 35, new Address("Berlin"))
        }.AsQueryable();
}
