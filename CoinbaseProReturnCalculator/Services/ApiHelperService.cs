using System.Net.Http;
using Newtonsoft.Json;

namespace CoinbaseProReturnCalculator
{
    public class ApiHelperService
    {
        private readonly HttpClient _client = new HttpClient();

        public decimal GetCurrentCryptoValue(string unit)
        {
            HttpResponseMessage response = _client.GetAsync("https://api.coinbase.com/v2/prices/" + unit + "-EUR/buy").Result;
            var resp = response.Content.ReadAsStringAsync().Result;
            var currentPrice = JsonConvert.DeserializeObject<BitCoinModel>(resp).data;
            return currentPrice.amount;
        }

        public decimal GetCurrentGbp(decimal euro)
        {
            HttpResponseMessage response = _client.GetAsync("https://api.exchangeratesapi.io/latest?symbols=GBP").Result;
            var resp = response.Content.ReadAsStringAsync().Result;
            var currentPrice = JsonConvert.DeserializeObject<Euro>(resp).rates;

            var gbpPrice = euro * currentPrice.GBP;
            return gbpPrice;
        }
    }

    public class Euro
    {
        public Rates rates { get; set; }
    }

    public class Rates
    {
        public decimal GBP { get; set; }
    }

    public class BitCoin
    {
        public decimal amount { get; set; }
    }

    public class BitCoinModel
    {
        public BitCoin data { get; set; }
    }

}