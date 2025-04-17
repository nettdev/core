namespace Nett.Core.Models;

[ExcludeFromCodeCoverage]
public sealed record Email(
    string Address,
    string? Provider = null
);