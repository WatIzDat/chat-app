using SharedKernel;

namespace Web.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot convert a success result to a ProblemDetails response.");
        }

        return Results.Problem(
            statusCode: GetStatusCode(result.Error.Type),
            title: GetTitle(result.Error.Type),
            type: GetType(result.Error.Type),
            extensions: new Dictionary<string, object?>
            {
                { "errors", new[] { result.Error } }
            });

        static int GetStatusCode(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        static string GetTitle(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                ErrorType.Conflict => "Conflict",
                _ => "Internal Server Error"
            };
        }

        static string GetType(ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => "https://www.rfc-editor.org/rfc/rfc9110.html#name-400-bad-request",
                ErrorType.NotFound => "https://www.rfc-editor.org/rfc/rfc9110.html#name-404-not-found",
                ErrorType.Conflict => "https://www.rfc-editor.org/rfc/rfc9110.html#name-409-conflict",
                _ => "https://www.rfc-editor.org/rfc/rfc9110.html#name-500-internal-server-error"
            };
        }
    }
}
