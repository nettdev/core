using System.Globalization;

namespace Nett.Core.Extensions;

public static class StringExtensions
{
    public static string CapitalizeFirstLetter(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return value!;

        ReadOnlySpan<char> src = value.AsSpan();
        Span<char> buffer = src.Length <= 256 ? stackalloc char[src.Length] : new char[src.Length];

        src.CopyTo(buffer);
        buffer[0] = char.ToUpper(buffer[0], CultureInfo.CurrentCulture);
        return new string(buffer);
    }
}
