using System.Linq.Expressions;
using Nett.Core.Result;
using Nett.Core.Validations;
using Shouldly;

namespace Nett.Core.UnitTest.Validations;

[ExcludeFromCodeCoverage]
public class ParameterRuleBuilderTests
{
    public class TestDto
    {
        public string Name { get; set; } = default!;
        public string FieldName = default!;
        public int Age { get; set; }
    }

    public enum TestEnum
    {
        Red = 1,
        Green = 2
    }

    [Fact]
    public void NotNull_WhenValueIsNull_ShouldAddErrorWithDefaultMessage()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", null!, errors);

        builder.NotNull();

        errors.ShouldHaveSingleItem();
        errors.Single().Code.ShouldBe("Field.NotNull");
    }

    [Fact]
    public void NotNull_WhenValueNotNull_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "value", errors);

        builder.NotNull();

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void NotNull_WhenCustomMessage_ShouldUseCustomMessage()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", null!, errors);

        builder.NotNull("Custom null message");

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Custom null message");
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    public void NotEmpty_WhenInvalidString_ShouldAddError(string value)
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", value, errors);

        builder.NotEmpty();

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Field cannot be empty");
    }

    [Fact]
    public void NotEmpty_WhenValidString_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "valid", errors);

        builder.NotEmpty();

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void MinLength_WhenStringShorterThanMin_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "abc", errors);

        builder.MinLength(5);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Field must have more than 5 characters");
    }

    [Fact]
    public void MinLength_WhenNonString_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Field", 123, errors);

        builder.MinLength(5);

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void MaxLength_WhenStringLongerThanMax_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "1234567890", errors);

        builder.MaxLength(5);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Field must have max 5 characters");
    }

    [Fact]
    public void Length_WhenStringOutOfRange_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "abc", errors);

        builder.Length(5, 10);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Field must be between 5 and 10");
    }

    [Fact]
    public void GreaterThan_WhenValueNotGreater_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Age", 18, errors);

        builder.GreaterThan(20);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Age must be greater than 20");
    }

    [Fact]
    public void LessThan_WhenValueNotLess_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Age", 25, errors);

        builder.LessThan(20);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Age must be less than 20 ");
    }

    [Fact]
    public void GreaterThanOrEqual_WhenValueLess_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Age", 19, errors);

        builder.GreaterThanOrEqual(20);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Age must be greater or equal to 20");
    }

    [Fact]
    public void LessThanOrEqualTo_WhenValueGreater_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Age", 21, errors);

        builder.LessThanOrEqualTo(20);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Age must be less or equal to 20");
    }

    [Fact]
    public void NotEqual_WhenValuesEqual_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "test", errors);

        builder.NotEqual("test");

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Field cannot be equal to test");
    }

    [Fact]
    public void Email_WhenInvalid_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Email", "invalid", errors);

        builder.Email();

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Email is an invalid e-mail address");
    }

    [Fact]
    public void Email_WhenValid_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Email", "test@example.com", errors);

        builder.Email();

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void CreditCard_WhenInvalid_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Card", "123", errors);

        builder.CreditCard();

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Card is an invalid credit card number");
    }

    [Fact]
    public void CreditCard_WhenValid_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Card", "4111111111111111", errors);

        builder.CreditCard();

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void CreditCard_WhenWithSpaces_ShouldIgnoreSpaces()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Card", "4111 1111 1111 1111", errors);

        builder.CreditCard();

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void IsInEnum_WhenInvalidEnumValue_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<TestEnum>("Color", (TestEnum)999, errors);

        builder.IsInEnum();

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Color is no a valid option");
    }

    [Fact]
    public void IsInEnum_WhenValidEnumValue_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<TestEnum>("Color", TestEnum.Red, errors);

        builder.IsInEnum();

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void IsInEnum_WhenNull_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<TestEnum?>("Color", null!, errors);

        builder.IsInEnum();

        errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(10, 2, 123.456)]
    [InlineData(5, 2, 532.111)]
    [InlineData(4, 2, 333.45)]
    [InlineData(5, 1, 222.22)]
    public void PrecisionScale_WhenInvalid_ShouldAddError(int precision, int scale, decimal value)
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<decimal>("Amount", value, errors);

        builder.PrecisionScale(precision, scale);

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe($"Amount must have precision {precision} and scale {scale}");
    }

    [Fact]
    public void PrecisionScale_WhenNonDecimal_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Field", 123, errors);

        builder.PrecisionScale(10, 2);

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void ExclusiveBetween_WhenValueOutOfExclusiveRange_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Age", 10, errors);

        builder.ExclusiveBetween(10, 30);

        errors.ShouldHaveSingleItem();
        errors.Single().Code.ShouldBe("Age.InclusiveBetween");
    }

    [Fact]
    public void InclusiveBetween_WhenValueOutOfInclusiveRange_ShouldAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<int>("Age", 5, errors);

        builder.InclusiveBetween(10, 30);

        errors.ShouldHaveSingleItem();
        errors.Single().Code.ShouldBe("Age.InclusiveBetween");
    }

    [Fact]
    public void Must_WhenPredicateFalse_ShouldAddErrorWithCustomMessage()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "test", errors);

        builder.Must(_ => false, "Custom must fail message");

        errors.ShouldHaveSingleItem();
        errors.Single().Message.ShouldBe("Custom must fail message");
        errors.Single().Code.ShouldBe("Field.Must");
    }

    [Fact]
    public void Must_WhenPredicateTrue_ShouldNotAddError()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "test", errors);

        builder.Must(v => v.Length > 0, "");

        errors.ShouldBeEmpty();
    }

    [Fact]
    public void MultipleValidators_ShouldAccumulateErrors()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "", errors);

        builder.NotEmpty();
        builder.MinLength(3);

        errors.Select(e => e.Code).ShouldBe(["Field.NotEmpty", "Field.MinLength"]);
    }

    [Fact]
    public void Build_WhenNoErrors_ShouldInvokeSuccessFunc()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "valid", errors);
        bool successCalled = false;
        TestDto? entity = null;

        var result = builder.Build(() => { successCalled = true; entity = new TestDto(); return entity; });

        successCalled.ShouldBeTrue();
    }

    [Fact]
    public void Build_WhenHasErrors_ShouldNotInvokeSuccessFunc()
    {
        var errors = new List<Error>();
        var builder = new ParameterRuleBuilder<string>("Field", "", errors);
        bool successCalled = false;

        builder.NotEmpty();

        var result = builder.Build<object>(() => { successCalled = true; return null!; });

        successCalled.ShouldBeFalse();
        errors.ShouldNotBeEmpty();
    }

    [Fact]
    public void RuleFor_SupportedPropertyExpression_ShouldInitializeWithCorrectFieldAndValue()
    {
        var dto = new TestDto { Name = "ab" };
        var constant = Expression.Constant(dto);
        var property = typeof(TestDto).GetProperty(nameof(TestDto.Name))!;
        var memberAccess = Expression.MakeMemberAccess(constant, property);
        var lambda = Expression.Lambda<Func<string>>(memberAccess);

        var errors = new List<Error>();

        var builder = ParameterRuleBuilder.RuleFor(lambda, errors);
        builder.MinLength(5);

        errors.ShouldHaveSingleItem();
        var error = errors.Single();
        error.Code.ShouldBe("Name.MinLength");
        error.Message.ShouldBe("Name must have more than 5 characters");
    }

    [Fact]
    public void RuleFor_SupportedFieldExpression_ShouldInitializeWithCorrectFieldAndValue()
    {
        var dto = new TestDto { FieldName = "ab" };
        var constant = Expression.Constant(dto);
        var field = typeof(TestDto).GetField(nameof(TestDto.FieldName))!;
        var memberAccess = Expression.MakeMemberAccess(constant, field);
        var lambda = Expression.Lambda<Func<string>>(memberAccess);

        var errors = new List<Error>();

        var builder = ParameterRuleBuilder.RuleFor(lambda, errors);
        builder.MinLength(5);

        errors.ShouldHaveSingleItem();
        var error = errors.Single();
        error.Code.ShouldStartWith("FieldName.MinLength");
        error.Message.ShouldBe("FieldName must have more than 5 characters");
    }

    [Fact]
    public void RuleFor_ConstantExpression_ShouldThrowArgumentException()
    {
        Expression<Func<string>> lambda = () => "hello";

        Should.Throw<ArgumentException>(() => ParameterRuleBuilder.RuleFor(lambda))
            .Message.ShouldContain("simple member access");
    }

    [Fact]
    public void RuleFor_ChainedRuleFor_ShouldShareErrorsList()
    {
        var dto = new TestDto { Name = "", Age = 0 };
        var nameConstant = Expression.Constant(dto);
        var nameProp = typeof(TestDto).GetProperty(nameof(TestDto.Name))!;
        var nameAccess = Expression.MakeMemberAccess(nameConstant, nameProp);
        var nameLambda = Expression.Lambda<Func<string>>(nameAccess);

        var ageConstant = Expression.Constant(dto);
        var ageProp = typeof(TestDto).GetProperty(nameof(TestDto.Age))!;
        var ageAccess = Expression.MakeMemberAccess(ageConstant, ageProp);
        var ageLambda = Expression.Lambda<Func<int>>(ageAccess);

        var errors = new List<Error>();

        var nameBuilder = ParameterRuleBuilder.RuleFor(nameLambda, errors);
        nameBuilder.NotEmpty();

        var ageBuilder = nameBuilder.RuleFor(ageLambda);
        ageBuilder.GreaterThan(18);

        errors.Select(e => e.Code).ShouldBe(["Name.NotEmpty", "Age.GreaterThan"]);
    }
}