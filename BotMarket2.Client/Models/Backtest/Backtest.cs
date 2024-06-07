using BotMarket2.Shared.DTO;
using BotMarket2.Client.Models.TradingStrategy;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BotMarket2.Client.Models.Backtest
{
    public class Backtest
    {
        private List<ITradingStrategy> strategies = new List<ITradingStrategy>();
        public List<HistoricalStockDataDTO> StockData { get; set; }
        public List<SignalResult> Results { get; set; }
        public AggregationMode AggregationMode { get; set; }
        public int ConfirmationThreshold { get; set; } = 2;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private IJSRuntime jsRuntime;

        public Backtest(List<HistoricalStockDataDTO> stockData, IJSRuntime jsRuntime)
        {
            StockData = stockData;
            Results = new List<SignalResult>();
            AggregationMode = AggregationMode.MajorityVote;
            this.jsRuntime = jsRuntime;
        }

        public void SetDateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
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
            var dailySignals = new List<SignalResult>();
            var filteredStockData = StockData.Where(data => data.Date >= StartDate && data.Date <= EndDate).ToList();
            foreach (var data in filteredStockData)
            {
                List<SignalResult> dailyResults = new List<SignalResult>();
                SignalResult? result = null;
                foreach (var strategy in strategies)
                {
                    if(!strategy.Name.Contains("MACD"))
                    {
                        bool? signal = strategy.EvaluateCurr(data);

                        result = new SignalResult(signal, data.CloseLast, data.Date, strategy.Name, strategy.SignalPriority);
                        dailyResults.Add(result);
                    }
                    else
                    {
                        if (filteredStockData.IndexOf(data) > 0)
                        {
                            bool? signal = strategy.EvaluatePrev(data, filteredStockData[filteredStockData.IndexOf(data) - 1]);

                            result = new SignalResult(signal, data.CloseLast, data.Date, strategy.Name, strategy.SignalPriority);
                            dailyResults.Add(result);
                        }
                    }
                    
                }

                if(dailyResults.Count == 0)
                {
                    dailyResults.Add(new(null, data.CloseLast, data.Date, "No Signal", 0));
                }
                result = AggregateSignals(dailyResults);
                yield return result;
                await Task.Delay(50);
            }

            Results.AddRange(dailySignals);
        }


        private SignalResult AggregateSignals(List<SignalResult> signals)
        {
            SignalResult finalSignal;
            switch (AggregationMode)
            {
                case AggregationMode.MajorityVote:
                    finalSignal = MajorityVote(signals);
                    break;
                case AggregationMode.SignalPriority:
                    finalSignal = SignalPriority(signals);
                    break;
                case AggregationMode.SignalConfirmation:
                    finalSignal = SignalConfirmation(signals);
                    break;
                default:
                    throw new InvalidOperationException("Invalid aggregation mode.");
            }

            return finalSignal;
        }

        private SignalResult MajorityVote(List<SignalResult> signals)
        {
            int buys = signals.Count(sr => sr.SignalType == SignalType.Buy);
            int sells = signals.Count(sr => sr.SignalType == SignalType.Sell);
            int holds = signals.Count(sr => sr.SignalType == SignalType.None);
            bool? isBuy = null;
            if (buys > sells && buys > holds)
                isBuy = true;
            else if (sells > buys && sells > holds)
                isBuy = false;
            return new SignalResult(isBuy, signals.First().Price, signals.First().Date, "Majority Vote", 0);
        }

        private SignalResult SignalPriority(List<SignalResult> signals)
        {
            return signals.OrderByDescending(sr => sr.StrategyPriority).First();
        }

        private SignalResult SignalConfirmation(List<SignalResult> signals)
        {
            int buyCount = signals.Count(sr => sr.SignalType == SignalType.Buy);
            int sellCount = signals.Count(sr => sr.SignalType == SignalType.Sell);
            if (buyCount >= ConfirmationThreshold)
                return new SignalResult(true, signals.First().Price, signals.First().Date, "Confirmed Buy", 0);
            if (sellCount >= ConfirmationThreshold)
                return new SignalResult(false, signals.First().Price, signals.First().Date, "Confirmed Sell", 0);

            return new SignalResult(null, signals.First().Price, signals.First().Date, "No Signal", 0);
        }
    }

    public enum AggregationMode
    {
        MajorityVote,
        SignalPriority,
        SignalConfirmation
    }
}
