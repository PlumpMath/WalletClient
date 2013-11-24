using Newtonsoft.Json;

namespace WalletClient.Shared.Model
{
    public class AddressValidation
    {
        [JsonProperty("isvalid")]
        public bool IsValid { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }
        
        [JsonProperty("ismine")]
        public bool IsMine { get; set; }

        [JsonProperty("account")]
        public string Account { get; set; }
    }
}
