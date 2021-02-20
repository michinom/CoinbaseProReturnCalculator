namespace CoinbaseProReturnCalculator.Models
{
    public class CoinbaseCsvModel
    {
        public string Portfolio { get; set; }
        public int TradeId { get; set; }
        public string Product { get; set; }
        public string Side { get; set; }
        public string CreatedAt { get; set; }
        public decimal Size { get; set; }
        public string SizeUnit { get; set; }
        public decimal Price { get; set; }
        public decimal Fee { get; set; }
        public decimal Total { get; set; }
        public string Pftu { get; set; }
    }
}
