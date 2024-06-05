using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class RSIStrategy : ITradingStrategy
    {
        public string Name => "RSI Strategy";
        public string Description => "Relative Strength Index Strategy. Buys when RSI is between threshold, else sell.";
        public int SignalPriority { get; set; }
        private double thresholdLower = 30;
        private double thresholdUpper = 70;

        public bool EvaluateCurr(HistoricalStockDataDTO data)
        {
            return data.RSI < thresholdLower || data.RSI > thresholdUpper;
        }

        public bool EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev)
        {
            return data.RSI < thresholdLower && prev.RSI > thresholdLower || data.RSI > thresholdUpper && prev.RSI < thresholdUpper;
        }

        public void UpdateParameters(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("ThresholdLower"))
                thresholdLower = Convert.ToDouble(parameters["ThresholdLower"]);
            if (parameters.ContainsKey("ThresholdUpper"))
                thresholdUpper = Convert.ToDouble(parameters["ThresholdUpper"]);
        }

        public (double, double) GetThresholds()
        {
            return (thresholdLower, thresholdUpper);
        }
    }

}
