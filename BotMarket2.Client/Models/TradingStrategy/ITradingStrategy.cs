using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.TradingStrategy
{
    public interface ITradingStrategy
    {
        string Name { get; }
        bool EvaluateCurr(HistoricalStockDataDTO data);
        bool EvaluatePrev(HistoricalStockDataDTO data, HistoricalStockDataDTO prev);
        void UpdateParameters(Dictionary<string, object> parameters);
    }
}
