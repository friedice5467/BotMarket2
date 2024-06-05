using BotMarket2.Shared.DTO;
using BotMarket2.Client.Models.TradingStrategy;

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

        public void SetAdditionSettings(AggregationMode mode, int confirmationThreshold)
        {
            AggregationMode = mode;
            ConfirmationThreshold = confirmationThreshold;
        }

        public async IAsyncEnumerable<SignalResult> RunAsync()
        {
            var dailySignals = new Dictionary<DateTime, List<SignalResult>>();

            foreach (var data in StockData)
            {
                List<SignalResult> dailyResults = new List<SignalResult>();
                foreach (var strategy in strategies)
                {
                    bool signal = strategy.EvaluateCurr(data);
                    var result = new SignalResult(signal, data.CloseLast, data.Date, strategy.Name, strategy.SignalPriority);
                    dailyResults.Add(result);
                    yield return result; 
                    await Task.Delay(50); 
                }
                dailySignals[data.Date] = dailyResults;
            }

            Results.AddRange(AggregateSignals(dailySignals));
        }

        private IEnumerable<SignalResult> AggregateSignals(Dictionary<DateTime, List<SignalResult>> dailySignals)
        {
            foreach (var entry in dailySignals)
            {
                SignalResult? finalSignal = null;
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
                {
                    yield return finalSignal; 
                }
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
