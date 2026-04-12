using System.Text.Json.Serialization;

namespace Nett.Core.Results;

[ExcludeFromCodeCoverage]
public sealed class Error
{
    public required string Message { get; init; }
    public string Code { get; init; } = "Generic.Error";
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Severity Severity { get; init; } = Severity.Warning;
    [JsonIgnore]
    public IList<Error>? InnerErrors { get; set; }

    public static implicit operator Error(string message) =>
        new() { Message = message };

    public static Error NotFound(string entity, string id) =>
        new() { Message = $"{entity} with id [{id}] not found", Code = $"{entity}.NotFound", Severity = Severity.Information };

    public static Error Conflict(string entity) =>
        new() { Message = $"{entity} already created", Code = $"{entity}.Conflict", Severity = Severity.Warning };

    public static Error Commit() =>
        new () { Message = "Error saving changes", Code = "Database.Commit", Severity = Severity.Error };

    public static Error Validation(string message) =>
        new() { Message = message, Code = "Validation.Error", Severity = Severity.Warning };

    public static Error NotAllowed(string message, string entity) =>
        new() { Message = message, Code = $"{entity}.NotAllowed", Severity = Severity.Warning };

    public static Error Custom(string message) =>
        new() { Message = message, Code = "Generic.Error", Severity = Severity.Error };
}

public enum Severity
{
    Error,
    Warning,
    Information
}
