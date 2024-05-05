using Svix;
using System.Net;
using System.Text;

namespace Web.Api.Utility;

public static class WebhookUtility
{
    public static async Task<IResult> VerifyWebhookAsync(
        HttpContext context,
        HttpRequest request,
        string secretConfigKey,
        Func<string, Task<IResult>> onSuccess,
        CancellationToken cancellationToken = default)
    {
        IConfiguration configuration = context.RequestServices.GetService<IConfiguration>()!;

        string? webhookSecret = configuration.GetValue<string>(secretConfigKey)
            ?? throw new Exception("Webhook secret not set.");

        string? svixId = request.Headers["svix-id"];
        string? svixTimestamp = request.Headers["svix-timestamp"];
        string? svixSignature = request.Headers["svix-signature"];

        if (svixId == null || svixTimestamp == null || svixSignature == null)
        {
            return Results.BadRequest("Svix headers were not set.");
        }

        WebHeaderCollection headers = [];

        headers.Set("svix-id", svixId);
        headers.Set("svix-timestamp", svixTimestamp);
        headers.Set("svix-signature", svixSignature);

        string body;

        using (StreamReader reader = new(request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync(cancellationToken);
        }

        Webhook webhook = new(webhookSecret);

        try
        {
            webhook.Verify(body, headers);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }

        return await onSuccess(body);
    }
}
