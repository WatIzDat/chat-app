using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedKernel.Utility;
using System.Security.Cryptography;
using System.Text;

namespace Web.Api.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
internal class ApiKeyAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!IsValidApiKey(context.HttpContext))
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private static bool IsValidApiKey(HttpContext context)
    {
        string? requestApiKeyHashBase64 = context.Request.Headers[ApiKeyHeaderName];

        if (string.IsNullOrWhiteSpace(requestApiKeyHashBase64) || !StringUtility.IsBase64(requestApiKeyHashBase64))
        {
            return false;
        }

        byte[] requestApiKeyHash = Convert.FromBase64String(requestApiKeyHashBase64);

        IConfiguration configuration = context.RequestServices.GetService<IConfiguration>()!;

        string actualApiKey = configuration.GetValue<string>("ApiKey")!;

        byte[] secretKey = Convert.FromBase64String(configuration.GetValue<string>("HashKey")!);

        byte[] actualApiKeyHash;

        using (HMACSHA256 hmac = new(secretKey))
        {
            actualApiKeyHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(actualApiKey));
        }

        return requestApiKeyHash == actualApiKeyHash;
    }
}
