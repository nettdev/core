namespace Nett.Core.Models;

[ExcludeFromCodeCoverage]
public sealed record Address(
    string Street,
    string City,
    string State,
    string PostalCode,
    string Country
);