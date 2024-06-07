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
            if (data.BollingerBandUpper == null || data.BollingerBandLower == null)
                return null;

            return (double)data.CloseLast > data.BollingerBandUpper + thresholdOver || (double)data.CloseLast < data.BollingerBandLower - thresholdUnder;
        }

        public bool EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev)
        {
            if (data.BollingerBandUpper == null || data.BollingerBandLower == null || prev.BollingerBandLower == null || prev.BollingerBandUpper == null)
                return false;

            return (double)prev.CloseLast < prev.BollingerBandLower - thresholdUnder && (double)data.CloseLast > data.BollingerBandUpper + thresholdOver;
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
