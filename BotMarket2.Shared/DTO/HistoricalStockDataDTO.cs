using BotMarket2.Client.Models.MACD;

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
        public double? SMA { get; set; }
        public double? EMA { get; set; }
        public double? RSI { get; set; }
        public MacdResultDTO? MACD { get; set; }
        public double? BollingerBandUpper { get; set; }
        public double? BollingerBandLower { get; set; }
        public decimal? SupportLevel { get; set; }
        public decimal? ResistanceLevel { get; set; }
    }
}
