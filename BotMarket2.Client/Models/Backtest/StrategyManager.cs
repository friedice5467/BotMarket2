using BotMarket2.Client.Models.TradingStrategy;

namespace BotMarket2.Client.Models.Backtest
{
    public class StrategyManager
    {
        private List<ITradingStrategy> strategies = new List<ITradingStrategy>();

        public void AddStrategy(ITradingStrategy strategy)
        {
            strategies.Add(strategy);
        }

        public void ClearStrategies()
        {
            strategies.Clear();
        }

        public IEnumerable<ITradingStrategy> GetStrategies()
        {
            return strategies.AsEnumerable();
        }
    }

}
