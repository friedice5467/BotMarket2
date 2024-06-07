namespace BotMarket2.Client.Models
{
    public class Transaction
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
    }

    public enum TransactionType
    {
        Buy,
        Sell
    }
}
