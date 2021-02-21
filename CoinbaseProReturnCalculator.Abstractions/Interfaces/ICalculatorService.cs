using System.Collections.Generic;
using System.Threading.Tasks;
using CoinbaseProReturnCalculator.Abstractions.Models;

namespace CoinbaseProReturnCalculator.Abstractions.Interfaces
{
    public interface ICalculatorService
    {
        Task<TableRow> CalculateReturn(IEnumerable<CoinbaseCsvModel> records, string crypto, string price);
    }
}