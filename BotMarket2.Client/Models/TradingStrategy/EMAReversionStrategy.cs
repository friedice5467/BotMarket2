using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class EMAReversionStrategy : ITradingStrategy
    {
        public string Name => "EMA Reversion Strategy";
        public string Description => "Buy when the price is below the EMA and sell when the price is above the EMA, configurable by threshold.";
        public int SignalPriority { get; set; }
        private double thresholdOver = 0.05;
        private double thresholdUnder = 0.05;

        public bool? EvaluateCurr(HistoricalStockDataDTO data)
        {
            if (data.EMA == null)
                return null;

            return (double)data.CloseLast < data.EMA * (1 - thresholdUnder);
        }

        public bool EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev)
        {
            if (data.EMA == null || prev.EMA == null)
                return false;

            return (double)prev.CloseLast > prev.EMA * (1 + thresholdOver) && (double)data.CloseLast < data.EMA * (1 - thresholdOver);
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
