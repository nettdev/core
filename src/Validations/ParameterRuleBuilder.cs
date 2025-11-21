using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Nett.Core.Extensions;
using Nett.Core.Result;

namespace Nett.Core.Validations;

public partial class ParameterRuleBuilder
{
    public static ParameterRuleBuilder<TValue> RuleFor<TValue>(Expression<Func<TValue>> expression, List<Error>? errors = null)
    {
        if (expression.Body is not MemberExpression memberExpression)
            throw new ArgumentException("Expression must be a simple member access", nameof(expression));

        string fieldName = memberExpression.Member.Name.CapitalizeFirstLetter();
        TValue value = GetValue<TValue>(expression.Body);
        return new ParameterRuleBuilder<TValue>(fieldName, value, errors ?? []);
    }

    private static TNext GetValue<TNext>(Expression expression)
    {
        return expression switch
        {
            MemberExpression { Expression: ConstantExpression expr } m when m.Member is FieldInfo f => (TNext)f.GetValue(expr.Value)!,
            MemberExpression { Expression: ConstantExpression expr } member when member.Member is PropertyInfo p => (TNext)p.GetValue(expr.Value)!,
            ConstantExpression constantExpression => (TNext)constantExpression.Value!,
            _ => throw new NotSupportedException("Unsupported expression type for AOT-safe evaluation.")
        };
    }
}

public partial class ParameterRuleBuilder<T>(string fieldName, T value, List<Error> errors)
{
    private readonly string _fieldName = fieldName;
    private readonly T _value = value;
    private readonly List<Error> _errors = errors;

    public ParameterRuleBuilder<T> Must(Func<T, bool> predicate, string message)
    {
        if (!predicate(_value))
        {
            AddError(nameof(Must), "", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> NotEmpty(string? message = null)
    {
        if (_value is string s && string.IsNullOrWhiteSpace(s))
        {
            AddError(nameof(NotEmpty), $"{_fieldName} cannot be empty", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> MinLength(int length, string? message = null)
    {
        if (_value is string s && s.Length < length)
        {
            AddError(nameof(MinLength), $"{_fieldName} must have more than {length} characters", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> GreaterThan(IComparable min, string? message = null)
    {
        if (_value is IComparable cmp && cmp.CompareTo(min) <= 0)
        {
            AddError(nameof(GreaterThan), $"{_fieldName} must be greater than {min}", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> LessThanOrEqualTo(IComparable max, string? message = null)
    {
        if (_value is IComparable cmp && cmp.CompareTo(max) > 0)
        {
            AddError(nameof(LessThanOrEqualTo), $"{_fieldName} must be less or equal to {max}", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> NotNull(string? message = null)
    {
        if (_value == null)
        {
            AddError(nameof(NotNull), $"{_fieldName} cannot be null", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> NotEqual(T other, string? message = null)
    {
        if (Equals(_value, other))
        {
            AddError(nameof(NotEqual), $"{_fieldName} cannot be equal to {other}", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> Length(int min, int max, string? message = null)
    {
        if (_value is string s && (s.Length < min || s.Length > max))
        {
            AddError(nameof(Length), $"{_fieldName} must be between {min} and {max}", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> MaxLength(int length, string? message = null)
    {
        if (_value is string s && s.Length > length)
        {
            AddError(nameof(MaxLength), $"{_fieldName} must have max {length} characters", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> LessThan(IComparable max, string? message = null)
    {
        if (_value is IComparable cmp && cmp.CompareTo(max) >= 0)
        {
            AddError(nameof(LessThan), $"{_fieldName} must be less than {max} ", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> GreaterThanOrEqual(IComparable min, string? message = null)
    {
        if (_value is IComparable cmp && cmp.CompareTo(min) < 0)
        {
            AddError(nameof(GreaterThanOrEqual), $"{_fieldName} must be greater or equal to {min}", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> Email(string? message = null)
    {
        if (_value is string s && !EmailRegex().IsMatch(s))
        {
            AddError(nameof(Email), $"{_fieldName} is an invalid e-mail address", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> CreditCard(string? message = null)
    {
        if (_value is string s && !CreditCardRegex().IsMatch(s.Replace(" ", "")))
        {
            AddError(nameof(CreditCard), $"{_fieldName} is an invalid credit card number", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> IsInEnum(string? message = null)
    {
        if (_value != null && !Enum.IsDefined(typeof(T), _value))
        {
            AddError(nameof(IsInEnum), $"{_fieldName} is no a valid option", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> ExclusiveBetween(IComparable from, IComparable to, string? message = null)
    {
        if (_value is IComparable cmp && (cmp.CompareTo(from) <= 0 || cmp.CompareTo(to) >= 0))
        {
            AddError(nameof(InclusiveBetween), $"{_fieldName} must be greater than {from} and less than {to}", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> InclusiveBetween(IComparable from, IComparable to, string? message = null)
    {
        if (_value is IComparable cmp && (cmp.CompareTo(from) < 0 || cmp.CompareTo(to) > 0))
        {
            AddError(nameof(InclusiveBetween), $"{_fieldName} must be greater or equal than {from} and less or equal to {to}", message);
        }
        return this;
    }

    public ParameterRuleBuilder<T> PrecisionScale(int precision, int scale, string? message = null)
    {
        if (_value is decimal dec)
        {
            var parts = dec.ToString(CultureInfo.InvariantCulture).Split('.');
            int integerDigits = parts[0].TrimStart('-').Length;
            int fractionalDigits = parts.Length > 1 ? parts[1].Length : 0;

            if (integerDigits + fractionalDigits > precision || fractionalDigits > scale)
            {
                AddError(nameof(PrecisionScale), $"{_fieldName} must have precision {precision} and scale {scale}", message);
            }
        }
        return this;
    }

    public ParameterRuleBuilder<TNext> RuleFor<TNext>(Expression<Func<TNext>> expr)
    {
        return ParameterRuleBuilder.RuleFor(expr, _errors);
    }

    public Result<TEntity, Error> Build<TEntity>(Func<TEntity> success)
    {
        if (_errors.Count == 0)
            return success();

        return new Error() 
        { 
            Message = "One or more validation errors ocour",
            Code = "Validation.Errors",
            Type = ErrorType.Validation,
            InnerErrors = _errors 
        };
    }

    private void AddError(string validator, string defaultMessage, string? customMessage = null)
    {
        _errors.Add(new() { Code = $"{_fieldName}.{validator}", Message = customMessage ?? defaultMessage });
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex(@"^\d{13,19}$")]
    private static partial Regex CreditCardRegex();
}
