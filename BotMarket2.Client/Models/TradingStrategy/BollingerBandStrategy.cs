using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public class BollingerBandStrategy : ITradingStrategy
    {
        public string Name => "Bollinger Band Strategy";
        public string Description => "Bollinger Band Strategy, buying/selling at thresholds";
        public int SignalPriority { get; set; }
        private double thresholdOver = 0.05;
        private double thresholdUnder = 0.05;

        public bool? EvaluateCurr(HistoricalStockDataDTO data)
        {
            if (!data.BollingerBandUpper.HasValue || !data.BollingerBandLower.HasValue)
                return null;

            if ((double)data.CloseLast > data.BollingerBandUpper + thresholdOver)
                return false;

            if ((double)data.CloseLast < data.BollingerBandLower - thresholdUnder)
                return true; 

            return null;
        }

        public bool? EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev)
        {
            if (data.BollingerBandUpper == null || data.BollingerBandLower == null || prev.BollingerBandLower == null || prev.BollingerBandUpper == null)
                return false;

            return (double)prev.CloseLast < prev.BollingerBandLower - thresholdUnder && (double)data.CloseLast > data.BollingerBandUpper + thresholdOver;
        }

        public void UpdateParameters(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey("ThresholdUpper"))
                thresholdOver = Convert.ToDouble(parameters["ThresholdUpper"]);
            if (parameters.ContainsKey("ThresholdLower"))
                thresholdUnder = Convert.ToDouble(parameters["ThresholdLower"]);
        }

        public (double, double) GetThresholds()
        {
            return (thresholdUnder, thresholdOver);
        }
    }
}
