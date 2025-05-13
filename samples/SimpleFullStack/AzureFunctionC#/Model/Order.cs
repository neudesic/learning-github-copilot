using System.Text.Json.Serialization;
using Newtonsoft.Json;


namespace api.Model
{
    public class Order
    {

        [JsonPropertyName("orderId")]
        [JsonProperty("orderId")]
        public required string OrderId { get; set; }

        [JsonPropertyName("companyId")]
        [JsonProperty("companyId")]
        public required string CompanyId { get; set; }

        [JsonPropertyName("customerName")]
        [JsonProperty("customerName")]
        public required string CustomerName { get; set; }

        [JsonPropertyName("orderDate")]
        [JsonProperty("orderDate")]
        public DateTime OrderDate { get; set; }

        [JsonPropertyName("totalAmount")]
        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }
    }
}