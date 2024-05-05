namespace Web.Api.Mappings;

public class DeleteUserWebhookMapping
{
    public DataMapping Data { get; set; }

    public class DataMapping
    {
        public string Id { get; set; }
    }
}
