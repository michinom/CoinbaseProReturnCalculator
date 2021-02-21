using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoinbaseProReturnCalculator.Abstractions.Models;

namespace CoinbaseProReturnCalculator.Abstractions.Extensions
{
    public static class CurrencyModelExtensions
    {
        public static Dictionary<string, decimal> ToRatesMap(this Rates currencyRates)
        {
            var currencyRatesMap = currencyRates.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => prop.Name,
                    prop => (decimal?)prop.GetValue(currencyRates, null) ?? 0m);

            return currencyRatesMap;

        }
        
        public static List<string> ToRatesList(this Rates currencyRates)
        {
            var currencyRatesList = currencyRates.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(prop => prop.Name)
                .ToList();

            return currencyRatesList;
        }
    }
}