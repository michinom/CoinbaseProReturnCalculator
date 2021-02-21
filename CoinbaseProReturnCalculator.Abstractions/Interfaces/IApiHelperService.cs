using System.Threading.Tasks;
using CoinbaseProReturnCalculator.Abstractions.Models;

namespace CoinbaseProReturnCalculator.Abstractions.Interfaces
{
    // These interfaces are here so you can use dependency injection in future, if you want to explore that.
    public interface IApiHelperService
    {
        Task<decimal> GetCurrentCryptoValue(string unit, string price);
        Task<CurrencyModel> GetExchangeRates();
    }
}