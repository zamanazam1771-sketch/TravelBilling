namespace TravelBilling.Application.Common;

public sealed record ApplicationError(string Code, string Message)
{
    public static readonly ApplicationError None = new(string.Empty, string.Empty);

    public static ApplicationError Validation(string message) => new("validation_error", message);
    public static ApplicationError NotFound(string message) => new("not_found", message);
    public static ApplicationError Conflict(string message) => new("conflict", message);
}
