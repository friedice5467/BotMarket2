namespace BotMarket2.Client.Models.Backtest
{
    public class SignalResult
    {
        public SignalResult(bool? isBuy, decimal price, DateTime date, string name, int strategyPriority)
        {
            SignalType = isBuy.HasValue ? isBuy.Value ? SignalType.Buy : SignalType.Sell : SignalType.None;
            Price = price;
            Date = date;
            Name = name;
            StrategyPriority = strategyPriority;
        }
        public SignalType SignalType { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string SignalTypeString => SignalType == SignalType.None ? "No Signal" : SignalType == SignalType.Buy ? "Buy" : "Sell";
        public int StrategyPriority { get; set; }
    }

    public enum SignalType
    {
        Buy,
        Sell,
        None
    }
}
