using BotMarket2.Common.Models;
using BotMarket2.Data;
using BotMarket2.Shared;

namespace BotMarket2.DAL.Data.Repository.StockData
{
    public class StockDataRepo : IStockDataRepo
    {
        private readonly ApplicationDbContext _context;
        public StockDataRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HistoricalStockData> GetStockData(string symbol)
        {
            return _context.HistoricalStockData.Where(x => x.Symbol == symbol).ToList();
        }

        public IEnumerable<HistoricalStockData> GetStockData(string symbol, int yrsFromEndDate)
        {
            var endDate = Consts.DATE_END_DATE;
            var startDate = endDate.AddYears(-yrsFromEndDate);
            return _context.HistoricalStockData.Where(x => x.Symbol == symbol && x.Date >= startDate && x.Date <= endDate).ToList();
        }
    }
}
