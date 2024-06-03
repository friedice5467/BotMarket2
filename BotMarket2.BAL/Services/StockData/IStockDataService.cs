using BotMarket2.Common.Models;
using BotMarket2.Shared.DTO;

namespace BotMarket2.BAL.Services.StockData
{
    public interface IStockDataService
    {
        IEnumerable<HistoricalStockData> GetStockData(string symbol);
        IEnumerable<HistoricalStockDataDTO> GetStockData(string symbol, int yrsFromEndDate);
    }
}