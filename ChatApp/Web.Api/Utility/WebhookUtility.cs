using Svix;
using System.Net;

namespace Web.Api.Utility
{
    public static class WebhookUtility
    {
        public static IResult? VerifyWebhook(string payload, HttpContext context, HttpRequest request)
        {
            IConfiguration configuration = context.RequestServices.GetService<IConfiguration>()!;

            string? webhookSecret = configuration.GetValue<string>("WebhookSecrets:RegisterUserSecret")
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

            Webhook webhook = new(webhookSecret);

            try
            {
                webhook.Verify(payload, headers);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }

            return null;
        }
    }
}
