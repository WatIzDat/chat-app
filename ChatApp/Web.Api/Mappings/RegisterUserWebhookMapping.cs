using System.Text.Json.Serialization;

namespace Web.Api.Mappings;

public class RegisterUserWebhookMapping
{
    public DataMapping Data { get; set; }

    public class DataMapping
    {
        public string Id { get; set; }

        public string Username { get; set; }

        [JsonPropertyName("email_addresses")]
        public EmailAddressMapping[] Email_Addresses { get; set; }

        [JsonPropertyName("primary_email_address_id")]
        public string PrimaryEmailAddressId { get; set; }

        public class EmailAddressMapping
        {
            [JsonPropertyName("email_address")]
            public string Email_Address { get; set; }
            
            public string Id { get; set; }
        }
    }
}

//public class RegisterUserWebhookData
//{
//    public string Id { get; set; }

//    public string Username { get; set; }

//    public string[] EmailAddresses { get; set; }
//}
