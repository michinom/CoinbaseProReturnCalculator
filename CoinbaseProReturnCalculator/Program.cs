using System;
using System.Linq;
using CoinbaseProReturnCalculator.Services;
using ConsoleTables;

namespace CoinbaseProReturnCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"Requested on {DateTime.Now.ToString($"dd'/'MM'/'yyyy' at 'HH':'mm':'ss")}");
            Console.WriteLine("Do you want to get Crypto's current 'Buy' or 'Sell' price?");
            var price = Console.ReadLine().ToLower();

            var csvParserService = new CsvParserService();
            var records = csvParserService.ReadCsvFile("fills.csv");
            var cryptoPortfolio = records
                .Select(a => a.SizeUnit)
                .Distinct()
                .ToList();

            var table = new ConsoleTable(
                "Name",
                "Amount",
                "Total Cost",
                "Current Value",
                "Gain/Loss",
                "%",
                "Crypto's Live Price",
                "Your Average Price (Weighted)"
                );

            foreach (var crypto in cryptoPortfolio)
            {
                var returnCalculator = new CalculatorService();
                var row = returnCalculator.CalculateReturn(records, crypto, price);
                table.AddRow(
                    $"{row.Name}",
                    $"{row.Amount}",
                    $"{row.Cost}",
                    $"{row.CurrentValue}",
                    $"{row.GainLoss}",
                    $"{row.PercentageDiff}",
                    $"{row.CurrentPrice}",
                    $"{row.WeightedAvg}"
                    );
            }

            table.Write(Format.Alternative);
        }
    }
}