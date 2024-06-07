namespace BotMarket2.Client.Models.MACD
{
    public class MacdResultDTO
    {
        public double? Macd { get; set; }
        public double? Signal { get; set; }
        public double? Histogram { get; set; }
        public double? FastEma { get; set; }
        public double? SlowEma { get; set; }
    }
}
