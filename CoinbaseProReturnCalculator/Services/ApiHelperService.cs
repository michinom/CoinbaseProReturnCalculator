using System.Net.Http;
using Newtonsoft.Json;

namespace CoinbaseProReturnCalculator.Services
{
    public class ApiHelperService
    {
        private readonly HttpClient _client = new HttpClient();

        public decimal GetCurrentCryptoValue(string unit, string price)
        {
            var uri = "https://api.coinbase.com/v2/prices/" + unit + "-EUR/" + price;

            string resp = ApiCall(uri);

            var currentPrice = JsonConvert.DeserializeObject<BitCoinModel>(resp).Data;
            return currentPrice.Amount;
        }

        public decimal GetCurrentGbp(decimal euro)
        {
            var uri = "https://api.exchangeratesapi.io/latest?symbols=GBP";

            var resp = ApiCall(uri);

            var currentPrice = JsonConvert.DeserializeObject<Euro>(resp).Rates;
            var gbpPrice = euro * currentPrice.Gbp;
            return gbpPrice;
        }

        private string ApiCall(string uri)
        {
            HttpResponseMessage response = _client.GetAsync(uri).Result;
            var resp = response.Content.ReadAsStringAsync().Result;
            return resp;
        }
    }

    public class Euro
    {
        [JsonProperty("rates")]
        public Rates Rates { get; set; }
    }

    public class Rates
    {
        [JsonProperty("GBP")]
        public decimal Gbp { get; set; }
    }

    public class BitCoin
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

    public class BitCoinModel
    {
        [JsonProperty("data")]
        public BitCoin Data { get; set; }
    }
}