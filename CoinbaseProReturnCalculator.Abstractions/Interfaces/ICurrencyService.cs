namespace CoinbaseProReturnCalculator.Abstractions.Interfaces
{
    public interface ICurrencyService
    {
        decimal ConvertToCurrency(decimal baseValue, string currencyCode);
    }
}