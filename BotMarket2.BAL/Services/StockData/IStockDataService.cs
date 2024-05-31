using BotMarket2.Common.Models;

namespace BotMarket2.BAL.Services.StockData
{
    public interface IStockDataService
    {
        IEnumerable<HistoricalStockData> GetStockData(string symbol);
        IEnumerable<HistoricalStockData> GetStockData(string symbol, int yrsFromEndDate);
    }
}