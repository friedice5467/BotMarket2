using BotMarket2.Client.Models.TradingStrategy;
using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.Backtest
{
    public class Backtest
    {
        private List<ITradingStrategy> strategies = new List<ITradingStrategy>();
        public List<HistoricalStockDataDTO> StockData { get; set; }
        public List<SignalResult> Results { get; set; }
        public AggregationMode AggregationMode { get; set; }
        public int ConfirmationThreshold { get; set; } = 1; 

        public Backtest(List<HistoricalStockDataDTO> stockData)
        {
            StockData = stockData;
            Results = new List<SignalResult>();
            AggregationMode = AggregationMode.MajorityVote;
        }

        public void ClearStrategies()
        {
            strategies.Clear();
        }

        public void AddStrategy(ITradingStrategy strategy)
        {
            strategies.Add(strategy);
        }

        public void Run()
        {
            var dailySignals = new Dictionary<DateTime, List<SignalResult>>();

            foreach (var data in StockData)
            {
                foreach (var strategy in strategies)
                {
                    bool signal = strategy.EvaluateCurr(data);
                    if (!dailySignals.ContainsKey(data.Date))
                        dailySignals[data.Date] = new();

                    dailySignals[data.Date].Add(new SignalResult(signal, data.CloseLast, data.Date, strategy.Name, strategy.SignalPriority));
                }
            }

            AggregateSignals(dailySignals);
        }

        private void AggregateSignals(Dictionary<DateTime, List<SignalResult>> dailySignals)
        {
            Results.Clear();
            foreach (var entry in dailySignals)
            {
                SignalResult? finalSignal;
                switch (AggregationMode)
                {
                    case AggregationMode.MajorityVote:
                        finalSignal = MajorityVote(entry.Value);
                        break;
                    case AggregationMode.SignalPriority:
                        finalSignal = SignalPriority(entry.Value);
                        break;
                    case AggregationMode.SignalConfirmation:
                        finalSignal = SignalConfirmation(entry.Value);
                        break;
                    default:
                        throw new InvalidOperationException("Invalid aggregation mode.");
                }

                if (finalSignal != null)
                    Results.Add(finalSignal);
            }
        }

        private SignalResult MajorityVote(List<SignalResult> signals)
        {
            int buys = signals.Count(sr => sr.SignalType == SignalType.Buy);
            int sells = signals.Count(sr => sr.SignalType == SignalType.Sell);
            bool isBuy = buys > sells;
            return new SignalResult(isBuy, signals.First().Price, signals.First().Date, "Majority Vote", 0);
        }

        private SignalResult SignalPriority(List<SignalResult> signals)
        {
            return signals.OrderByDescending(sr => sr.StrategyPriority).First();
        }

        private SignalResult? SignalConfirmation(List<SignalResult> signals)
        {
            int buyCount = signals.Count(sr => sr.SignalType == SignalType.Buy);
            int sellCount = signals.Count(sr => sr.SignalType == SignalType.Sell);
            if (buyCount >= ConfirmationThreshold)
                return new SignalResult(true, signals.First().Price, signals.First().Date, "Confirmed Buy", 0);
            if (sellCount >= ConfirmationThreshold)
                return new SignalResult(false, signals.First().Price, signals.First().Date, "Confirmed Sell", 0);

            return null; 
        }
    }
    public enum AggregationMode
    {
        MajorityVote,
        SignalPriority,
        SignalConfirmation
    }

}
