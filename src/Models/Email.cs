namespace Nett.Core.Models;

public sealed record Email(
    string Address,
    string? Provider = null
);