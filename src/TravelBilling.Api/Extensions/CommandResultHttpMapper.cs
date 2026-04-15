using TravelBilling.Api.Contracts.Common;
using TravelBilling.Application.Common;

namespace TravelBilling.Api.Extensions;

public static class CommandResultHttpMapper
{
    public static IResult ToHttpResult(this CommandResult result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok();
        }

        return result.Error.Code switch
        {
            "validation_error" => Results.BadRequest(new ApiErrorResponse(result.Error.Code, result.Error.Message)),
            "not_found" => Results.NotFound(new ApiErrorResponse(result.Error.Code, result.Error.Message)),
            "conflict" => Results.Conflict(new ApiErrorResponse(result.Error.Code, result.Error.Message)),
            _ => Results.BadRequest(new ApiErrorResponse(result.Error.Code, result.Error.Message))
        };
    }

    public static IResult ToHttpResult<T>(this CommandResult<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess && result.Value is not null)
        {
            return onSuccess(result.Value);
        }

        return result.ToHttpResult();
    }
}
