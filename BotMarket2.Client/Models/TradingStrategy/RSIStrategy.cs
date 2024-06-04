using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class RSIStrategy : ITradingStrategy
    {
        public string Name => "RSI Strategy";

        private int thresholdLower = 30;
        private int thresholdUpper = 70;

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
                thresholdLower = Convert.ToInt32(parameters["ThresholdLower"]);
            if (parameters.ContainsKey("ThresholdUpper"))
                thresholdUpper = Convert.ToInt32(parameters["ThresholdUpper"]);
        }
    }

}
