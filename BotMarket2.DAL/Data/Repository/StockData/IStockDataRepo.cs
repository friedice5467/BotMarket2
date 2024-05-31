using BotMarket2.Common.Models;

namespace BotMarket2.DAL.Data.Repository.StockData
{
    public interface IStockDataRepo
    {
        IEnumerable<HistoricalStockData> GetStockData(string symbol);
        IEnumerable<HistoricalStockData> GetStockData(string symbol, int yrsFromEndDate);
    }
}