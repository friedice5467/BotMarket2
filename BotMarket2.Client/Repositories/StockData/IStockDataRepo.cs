using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Repositories.StockData
{
    public interface IStockDataRepo
    {
        Task<IEnumerable<HistoricalStockDataDTO>> GetStockData(string symbol, int yrsFromEndDate);
        Task<IEnumerable<HistoricalStockDataDTO>> GetStockDataAll(string symbol);
    }
}