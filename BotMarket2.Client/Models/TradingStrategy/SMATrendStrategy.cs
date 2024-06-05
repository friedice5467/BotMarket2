using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class SMATrendStrategy : ITradingStrategy
    {
        public string Name => "SMA Trend Strategy";
        public string Description => "Buy when the stock price is above the SMA, else sell. Configurable by threshold.";
        private double thresholdOver = 0.05;
        private double thresholdUnder = 0.05;

        public bool EvaluateCurr(HistoricalStockDataDTO data)
        {
            if (data.SMA == null)
                return false;

            return (double)data.CloseLast > data.SMA * (1 + thresholdOver);
        }
        
        public bool EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev)
        {
            if (data.SMA == null || prev.SMA == null)
                return false;

            return (double)prev.CloseLast < prev.SMA * (1 - thresholdUnder) && (double)data.CloseLast > data.SMA * (1 + thresholdOver);
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
