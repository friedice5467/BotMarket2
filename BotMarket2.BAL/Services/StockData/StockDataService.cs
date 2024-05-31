using BotMarket2.Common.Models;
using BotMarket2.DAL.Data.Repository.StockData;

namespace BotMarket2.BAL.Services.StockData
{
    public class StockDataService : IStockDataService
    {
        private readonly IStockDataRepo _stockDataRepo;
        public StockDataService(IStockDataRepo stockDataRepo)
        {
            _stockDataRepo = stockDataRepo;
        }

        public IEnumerable<HistoricalStockData> GetStockData(string symbol)
        {
            return _stockDataRepo.GetStockData(symbol);
        }

        public IEnumerable<HistoricalStockData> GetStockData(string symbol, int yrsFromEndDate)
        {
            return _stockDataRepo.GetStockData(symbol, yrsFromEndDate);
        }
    }
}
