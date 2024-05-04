using Newtonsoft.Json;

namespace Web.Api.Mappings;

public class RegisterUserWebhookMapping
{
    public DataMapping Data { get; set; }

    public class DataMapping
    {
        public string Id { get; set; }

        public string Username { get; set; }

        [JsonProperty("email_addresses")]
        public EmailAddressMapping[] EmailAddresses { get; set; }

        [JsonProperty("primary_email_address_id")]
        public string PrimaryEmailAddressId { get; set; }

        public class EmailAddressMapping
        {
            [JsonProperty("email_address")]
            public string EmailAddress { get; set; }
            
            public string Id { get; set; }
        }
    }
}
