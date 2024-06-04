using BotMarket2.Client.Models.TradingStrategy;
using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.Backtest
{
    public class Backtest
    {
        private List<ITradingStrategy> strategies = new List<ITradingStrategy>();
        public List<HistoricalStockDataDTO> StockData { get; set; }
        public bool IsComparePrev { get; set; }
        public Backtest(List<HistoricalStockDataDTO> stockData)
        {
            StockData = stockData;
        }

        public void AddStrategy(ITradingStrategy strategy)
        {
            strategies.Add(strategy);
        }

        public void Run()
        {
            foreach (var data in StockData)
            {
                foreach (var strategy in strategies)
                {
                    if (strategy.EvaluateCurr(data) && !IsComparePrev)
                    {
                        Console.WriteLine($"Signal triggered by {strategy.Name} on {data.Date}: Current");
                    }
                    else if(strategy.EvaluatePrev(data, StockData[StockData.IndexOf(data) - 1]) && IsComparePrev)
                    {
                        Console.WriteLine($"Signal triggered by {strategy.Name} on {data.Date}: Previous");
                    }
                
                }
            }
        }
    }

}
