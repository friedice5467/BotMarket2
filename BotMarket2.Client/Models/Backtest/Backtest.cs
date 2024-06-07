using BotMarket2.Shared.DTO;
using BotMarket2.Client.Models.TradingStrategy;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BotMarket2.Client.Models.Backtest
{
    public class Backtest
    {
        private StrategyManager strategyManager = new StrategyManager();
        private SignalAggregator signalAggregator = new SignalAggregator();
        private PortfolioManager portfolioManager;
        public List<HistoricalStockDataDTO> StockData { get; set; }
        public AggregationMode AggregationMode { get; set; }
        public int ConfirmationThreshold { get; set; } = 2;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        private IJSRuntime jsRuntime;

        public Backtest(List<HistoricalStockDataDTO> stockData, PortfolioManager portfolio, IJSRuntime jsRuntime)
        {
            portfolioManager = portfolio;
            StockData = stockData;
            this.jsRuntime = jsRuntime;
        }

        public void SetDateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public void ClearStrategies()
        {
            strategyManager.ClearStrategies();
        }

        public void AddStrategy(ITradingStrategy strategy)
        {
            strategyManager.AddStrategy(strategy);
        }

        public void ConfigureAggregationSettings(AggregationMode mode, int confirmationThreshold)
        {
            AggregationMode = mode;
            ConfirmationThreshold = confirmationThreshold;
        }

        public async IAsyncEnumerable<SignalResult> RunAsync()
        {
            var filteredStockData = StockData.Where(data => data.Date >= StartDate && data.Date <= EndDate).ToList();
            foreach (var data in filteredStockData)
            {
                List<SignalResult> dailyResults = new List<SignalResult>();
                foreach (var strategy in strategyManager.GetStrategies())
                {
                    SignalResult? result = ProcessStrategy(filteredStockData, data, strategy);
                    if (result != null)
                        dailyResults.Add(result);
                }

                if (dailyResults.Count == 0)
                    dailyResults.Add(new(null, data.CloseLast, data.Date, "No Signal", 0));

                SignalResult aggregatedResult = signalAggregator.AggregateSignals(dailyResults, AggregationMode, ConfirmationThreshold);
                portfolioManager.ActOnSignal(data, aggregatedResult.SignalType);
                yield return aggregatedResult;
                await Task.Delay(50); 
            }
        }

        private SignalResult? ProcessStrategy(List<HistoricalStockDataDTO> filteredStockData, HistoricalStockDataDTO data, ITradingStrategy strategy)
        {
            if (!strategy.Name.Contains("MACD"))
            {
                bool? signal = strategy.EvaluateCurr(data);

                var result = new SignalResult(signal, data.CloseLast, data.Date, strategy.Name, strategy.SignalPriority);
                return result;
            }
            else
            {
                if (filteredStockData.IndexOf(data) > 0)
                {
                    bool? signal = strategy.EvaluatePrev(data, filteredStockData[filteredStockData.IndexOf(data) - 1]);

                    var result = new SignalResult(signal, data.CloseLast, data.Date, strategy.Name, strategy.SignalPriority);
                    return result;
                }
            }

            return null;
        }
    }
}
