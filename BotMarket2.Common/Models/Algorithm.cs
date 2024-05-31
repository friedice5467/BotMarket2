using Microsoft.EntityFrameworkCore;
namespace BotMarket2.Common.Models
{
    public class Algorithm
    {
        public int AlgorithmId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? SMA { get; set; }
        public int? EMA { get; set; }
        public int? RSI { get; set; }
        public int? MACD { get; set; }
        [Precision(18, 2)]
        public decimal? BollingerBands { get; set; }
        [Precision(18, 2)]
        public decimal? SupportLevel { get; set; }
        [Precision(18, 2)]
        public decimal? ResistanceLevel { get; set; }
        [Precision(18, 2)]
        public decimal? TrailingStop { get; set; }
        [Precision(18, 2)]
        public decimal? StopLoss { get; set; }
        [Precision(18, 2)]
        public decimal? TakeProfit { get; set; }
        public TimeSpan TradingHoursStart { get; set; }
        public TimeSpan TradingHoursEnd { get; set; }
        public int? HoldPeriod { get; set; } 
    }

}
