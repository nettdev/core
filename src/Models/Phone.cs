namespace Nett.Core.Models;

[ExcludeFromCodeCoverage]
public sealed record Phone(
    string DDI,
    string DDD,
    string Number
);