using System.Net.Http;
using System.Threading.Tasks;
using CoinbaseProReturnCalculator.Abstractions.Interfaces;
using CoinbaseProReturnCalculator.Abstractions.Models;
using Newtonsoft.Json;

namespace CoinbaseProReturnCalculator.Services
{
    public class ApiHelperService : IApiHelperService
    {
        private readonly HttpClient _httpClient;

        public ApiHelperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> GetCurrentCryptoValue(string unit, string price)
        {
            var uri = $"https://api.coinbase.com/v2/prices/{unit}-EUR/{price}";

            string resp = await ApiCall(uri);

            var currentPrice = JsonConvert.DeserializeObject<CoinbaseDataModel>(resp).Data;
            return currentPrice.Amount;
        }

        public async Task<CurrencyModel> GetExchangeRates()
        {
            const string uri = "https://api.exchangeratesapi.io/latest";

            var resp = await ApiCall(uri);

            var currencies = JsonConvert.DeserializeObject<CurrencyModel>(resp);
            return currencies;
        }

        private async Task<string> ApiCall(string uri)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            var resp = await response.Content.ReadAsStringAsync();
            return resp;
        }
    }

    
}