using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CoinbaseProReturnCalculator
{
    public class CalculatorService
    {
        public TableRows CalculateReturn(List<CoinbaseCsvModel> records, string crypto)
        {
            var cryptoUnit = records.Where(x => x.SizeUnit == crypto);
            var amount = cryptoUnit.Sum(x => x.Size);
            var totalSpend = cryptoUnit.Sum(x => x.Total * -1);

            /*
            if (crypto == "GRT")
            {
                amount += 8.41m;
            }
            */

            var coinbaseApiService = new ApiHelperService();
            var currentPrice = coinbaseApiService
                .GetCurrentCryptoValue(crypto);

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

            var tableRow = new TableRows
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

        private static string Gbp(decimal euroAmount)
        {
            var coinbaseApiService = new ApiHelperService();
            var gbpPrice = coinbaseApiService.GetCurrentGbp(euroAmount);
            return gbpPrice.ToString("C", new CultureInfo("en-GB"));
        }
    }
}