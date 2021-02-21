using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CoinbaseProReturnCalculator.Abstractions.Interfaces;
using CoinbaseProReturnCalculator.Abstractions.Models;

namespace CoinbaseProReturnCalculator.Services
{
    public class CalculatorService : ICalculatorService
    {
        
        private readonly IApiHelperService _apiHelperService;
        private readonly ICurrencyService _currencyService;
        
        // We passed these in so that we only need to instantiate them once.
        public CalculatorService(IApiHelperService apiHelperService, ICurrencyService currencyService)
        {
            _apiHelperService = apiHelperService;
            _currencyService = currencyService;
        }

        public async Task<TableRow> CalculateReturn(IEnumerable<CoinbaseCsvModel> records, string crypto, string price)
        {
            var cryptoUnit = records.Where(x => x.SizeUnit == crypto);
            var amount = cryptoUnit.Sum(x => x.Size);
            var totalSpend = cryptoUnit.Sum(x => x.Total * -1);
            
            var currentPrice = await _apiHelperService
                .GetCurrentCryptoValue(crypto, price);

            var weightAverageRows = 0m;
            foreach (var row in cryptoUnit)
            {
                var quantityTimesPrice = row.Size * row.Price;
                weightAverageRows += quantityTimesPrice;
            }

            var weightedAverage = weightAverageRows / amount;

            var currentValue = amount * currentPrice;
            var priceDiff = currentValue - totalSpend;
            var percDiff = ((currentValue - totalSpend) / totalSpend)
                .ToString("P", CultureInfo.InvariantCulture);

            var tableRow = new TableRow
            {
                Name = crypto,
                Amount = amount,
                Cost = $"{Gbp(totalSpend)} ({Euro(totalSpend)})",
                CurrentValue = $"{Gbp(currentValue)} ({Euro(currentValue)})",
                GainLoss = $"{Gbp(priceDiff)} ({Euro(priceDiff)})",
                PercentageDiff = $"{percDiff}",
                CurrentPrice = $"{Gbp(currentPrice)} ({Euro(currentPrice)})",
                WeightedAvg = $"{Gbp(weightedAverage)} ({Euro(weightedAverage)})"
            };

            return tableRow;
        }

        private static string Euro(decimal euroAmount)
        {
            return euroAmount.ToString("C", new CultureInfo("en-IE"));
        }

        private string Gbp(decimal baseAmount)
        {
            var gbpPrice = _currencyService.ConvertToCurrency(baseAmount, "GBP");
            return gbpPrice.ToString("C", new CultureInfo("en-GB"));
        }
    }
}