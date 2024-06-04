using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class MACDCrossoverStrategy : ITradingStrategy
    {
        public string Name => "MACD Crossover";
        private double thresholdOver = 0.05;
        private double thresholdUnder = 0.05;

        public bool EvaluateCurr(HistoricalStockDataDTO current)
        {
            if (current.MACD == null)
                return false;

            return current.MACD < -thresholdUnder || current.MACD > thresholdOver;
        }

        public bool EvaluatePrev(HistoricalStockDataDTO current, HistoricalStockDataDTO previous)
        {
            if (previous.MACD == null || current.MACD == null)
                return false;

            bool crossesUp = previous.MACD < -thresholdUnder && current.MACD > thresholdOver;
            bool crossesDown = previous.MACD > thresholdOver && current.MACD < -thresholdUnder;

            return crossesUp || crossesDown;
        }

        public void UpdateParameters(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("ThresholdOver"))
                thresholdOver = Convert.ToDouble(parameters["ThresholdOver"]);
            if (parameters.ContainsKey("ThresholdUnder"))
                thresholdUnder = Convert.ToDouble(parameters["ThresholdUnder"]);
        }
    }

}
