using System;
using CoinbaseProReturnCalculator.Abstractions.Interfaces;
using CoinbaseProReturnCalculator.Abstractions.Models;

namespace CoinbaseProReturnCalculator.Services
{
    public class CurrencyService : ICurrencyService
    {
        // this is static so it can be used in the lazy instantiation of the currency model.
        private readonly IApiHelperService _apiHelperService;
        
        // Lazy means that it will only be created when we ask for the value, and only once, rather than at instantiation time.
        private readonly Lazy<CurrencyModel> _currencyModelLazy;
        
        private CurrencyModel LazyCurrencyModel => _currencyModelLazy.Value;

        public CurrencyService(IApiHelperService apiHelperService)
        {
            _apiHelperService = apiHelperService;
            _currencyModelLazy = new Lazy<CurrencyModel>(GetCurrencyModels);
        }

        // Here I changed the method that converted Euro to GBP,
        // Allows any currency that the API can get, to be more useful to users in other locales.
        public decimal ConvertToCurrency(decimal baseValue, string currencyCode)
        {
            // This is simplified so we return zero if the currency doesn't exist.
            // You would ideally handle the check below before even getting here!
            var resultValue = 0m;
            
            var rates = LazyCurrencyModel.RatesMap;

            // Check the value exists in the dictionary!
            if (rates.TryGetValue(currencyCode, out var rate))
            {
                resultValue = baseValue * rate;
            }

            return resultValue;
        }

        private CurrencyModel GetCurrencyModels()
        {
            // This is kind of a hack as Lazy doesn't allow async.
            var currencyModel = _apiHelperService.GetExchangeRates().GetAwaiter().GetResult();
            return currencyModel;
        }
    }
}