using Newtonsoft.Json;

namespace CoinbaseProReturnCalculator.Abstractions.Models
{
    public class BitCoin
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

    public class CoinbaseDataModel
    {
        [JsonProperty("data")]
        public BitCoin Data { get; set; }
    }
}