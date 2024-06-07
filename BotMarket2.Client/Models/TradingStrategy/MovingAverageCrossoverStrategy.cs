using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class MovingAverageCrossoverStrategy : ITradingStrategy
    {
        public string Name => "Moving Average Crossover";
        public string Description => "Moving Average Crossover, buying/selling at thresholds";
        public int SignalPriority { get; set; }
        private double thresholdOver = 0.05;
        private double thresholdUnder = 0.05;

        public bool? EvaluateCurr(HistoricalStockDataDTO current)
        {
            if (current.EMA == null || current.SMA == null)
                return null;

            return current.EMA < current.SMA - thresholdUnder || current.EMA > current.SMA + thresholdOver;
        }

        public bool EvaluatePrev(HistoricalStockDataDTO current, HistoricalStockDataDTO previous)
        {
            if (previous.EMA == null || previous.SMA == null || current.EMA == null || current.SMA == null)
                return false;

            bool crossoverUp = previous.EMA < previous.SMA && current.EMA > current.SMA + thresholdOver;
            bool crossoverDown = previous.EMA > previous.SMA && current.EMA < current.SMA - thresholdUnder;

            return crossoverUp || crossoverDown;
        }

        public void UpdateParameters(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("ThresholdOver"))
                thresholdOver = Convert.ToDouble(parameters["ThresholdOver"]);
            if (parameters.ContainsKey("ThresholdUnder"))
                thresholdUnder = Convert.ToDouble(parameters["ThresholdUnder"]);
        }

        public (double, double) GetThresholds()
        {
            return (thresholdUnder, thresholdOver);
        }
    }

}
