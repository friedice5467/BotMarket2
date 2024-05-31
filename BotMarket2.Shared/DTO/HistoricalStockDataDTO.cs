namespace BotMarket2.Shared.DTO
{
    public class HistoricalStockDataDTO
    {
        public string? Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal CloseLast { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public long Volume { get; set; }
    }
}
