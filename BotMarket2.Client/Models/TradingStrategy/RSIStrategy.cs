using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class RSIStrategy : ITradingStrategy
    {
        public string Name => "RSI Strategy";
        public string Description => $"RSI levels below lower threshold, {thresholdLower}, generate buy signals and indicate an oversold or undervalued condition. RSI levels above threshold higher, {thresholdUpper}, generate sell signals and suggest that a security is overbought or overvalued. Current calculated RSI values are based on a 14 day period.";
        public int SignalPriority { get; set; }
        private double thresholdLower = 40;
        private double thresholdUpper = 70;

        /// <summary>
        /// If RSI is below thresholdLower, return true(buy), if above thresholdUpper, return false(sell), else return null and do nothing
        /// </summary>
        public bool? EvaluateCurr(HistoricalStockDataDTO data)
        {
            if (data.RSI.HasValue)
            {
                if (data.RSI.Value < thresholdLower)
                {
                    return true;
                }
                else if (data.RSI.Value > thresholdUpper)
                {
                    return false;
                }
            }
            return null;
        }

        public bool? EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev)
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
