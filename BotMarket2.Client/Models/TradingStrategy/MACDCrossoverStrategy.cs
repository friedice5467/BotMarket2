using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class MACDCrossoverStrategy : ITradingStrategy
    {
        public string Name => "MACD Crossover Strategy";
        public string Description => "When the MACD line crosses the zero line from below, it indicates an uptrend and signals to place buy or long orders. When the MAC line crosses the zero line from above, it indicates a downtrend and signals to place sell or short orders. For simplicity, threshold does nothing.";
        public int SignalPriority { get; set; }
        private double thresholdOver = 0.00;
        private double thresholdUnder = 0.00;

        public bool? EvaluateCurr(HistoricalStockDataDTO current)
        {
            return null;
        }


        public bool? EvaluatePrev(HistoricalStockDataDTO current, HistoricalStockDataDTO previous)
        {
            if (previous.MACD == null || current.MACD == null)
                return null;

            double? prevMacdValue = previous.MACD.Macd;
            double? currMacdValue = current.MACD.Macd;

            if (!prevMacdValue.HasValue || !currMacdValue.HasValue)
                return null;

            if (prevMacdValue <= 0 && currMacdValue > 0)
                return true; 
            else if (prevMacdValue >= 0 && currMacdValue < 0)
                return false; 

            return null; 
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
