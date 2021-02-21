using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoinbaseProReturnCalculator.Services;
using ConsoleTables;

namespace CoinbaseProReturnCalculator
{
    public class Program
    {
        // You can use one HttpClient for your whole app!
        // It'll manage the underlying threadpool/sockets for you:
        // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
        // In a Asp.NET application you would use IHttpClientFactory via Dependency Injection:
        // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        private static readonly HttpClient Client = new HttpClient();
        private static readonly string[] PriceOptions = new[] {"buy", "sell"};
        
        public static async Task Main(string[] args)
        {
            Console.WriteLine($"Requested on {DateTime.Now.ToString($"dd'/'MM'/'yyyy' at 'HH':'mm':'ss")}");

            string price;
            
            // You want to check that they selected a valid option. Loop until as such.
            do
            {
                // Price could be null, so check use null conditional operator before ToLower.
                Console.WriteLine("Do you want to get Crypto's current 'Buy' or 'Sell' price?");
                price = Console.ReadLine()?.ToLower();
            } while (string.IsNullOrWhiteSpace(price) || !PriceOptions.Contains(price));
            
            // Consider passing this file name in as an argument to the program itself instead.
            // Hard-coding the file location is a bad code smell.
            var records = CsvParserService.ReadCsvFile("fills.csv");
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

            // Look into a thing called "Dependency Injection" to simplify this series of instantiations below;
            // https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection
            // We also want the services instantiated outside the loop,
            // or we would be re-instantiating all the parts below in each iteration of the loop!
            var apiHelperService = new ApiHelperService(Client);
            var currencyService = new CurrencyService(apiHelperService);
            var returnCalculator = new CalculatorService(apiHelperService, currencyService);

            // This will parallelize (kinda) the calculations which includes an http call each time,
            // instead of doing them sequentially. This should make it faster when dealing with larger portfolios.
            // You won't really notice a difference with your sample portfolio though.
            var calculateTasks = cryptoPortfolio
                .Select(crypto => returnCalculator.CalculateReturn(records, crypto, price))
                .ToList();

            // Wait for all the calculations to complete. 
            var tableRows = await Task.WhenAll(calculateTasks);

            foreach (var row in tableRows)
            {
                // You don't need to use interpolated strings here. Most are strings anyway.
                table.AddRow(
                    row.Name,
                    row.Amount,
                    row.Cost,
                    row.CurrentValue,
                    row.GainLoss,
                    row.PercentageDiff,
                    row.CurrentPrice,
                    row.WeightedAvg
                );
            }

            table.Write(Format.Alternative);
        }
    }
}