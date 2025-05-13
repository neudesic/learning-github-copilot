using System.Text.Json.Serialization;
using Newtonsoft.Json;
using api.Model;


namespace api.Model
{
    public class OrderResponse
    {
        [JsonPropertyName("orders")]
        [JsonProperty("orders")]
        public List<Order> Orders { get; set; }

        [JsonPropertyName("totalAmount")]
        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }

    }
}