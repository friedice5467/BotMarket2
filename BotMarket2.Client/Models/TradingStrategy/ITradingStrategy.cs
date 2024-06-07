using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public interface ITradingStrategy
    {
        string Name { get; }
        string Description { get; }
        int SignalPriority { get; set; }
        bool? EvaluateCurr(HistoricalStockDataDTO data);
        bool? EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev);
        void UpdateParameters(Dictionary<string, object> parameters);
        /// <summary>
        /// P1 is the lower threshold, P2 is the upper threshold
        /// </summary>
        (double, double) GetThresholds();
    }
}
