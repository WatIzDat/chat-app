using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace Web.Api.Infrastructure.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class GetUserIdFromAccessTokenAttribute : Attribute, IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        string authorizationHeader = context.HttpContext.Request.Headers.Authorization.ToString();

        string accessTokenRaw = authorizationHeader.Split(' ')[1];

        JwtSecurityTokenHandler jwtHandler = new();

        JwtSecurityToken accessToken = jwtHandler.ReadJwtToken(accessTokenRaw);

        if (Guid.TryParse(accessToken.Payload["userId"].ToString()!, out Guid userId))
        {
            context.ActionArguments["userId"] = userId;
        }
        else
        {
            context.ActionArguments["userId"] = Guid.Empty;
        }
    }
}
