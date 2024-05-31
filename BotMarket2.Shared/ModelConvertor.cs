using BotMarket2.Common.Models;
using BotMarket2.Shared.DTO;

namespace BotMarket2.Shared
{
    public static class ModelConvertor
    {
        public static HistoricalStockDataDTO StockDataToDTO(HistoricalStockData stockData)
        {
            return new HistoricalStockDataDTO
            {
                Date = stockData.Date,
                Open = stockData.Open,
                High = stockData.High,
                Low = stockData.Low,
                CloseLast = stockData.CloseLast,
                Volume = stockData.Volume,
                Symbol = stockData.Symbol
            };
        }
    }
}
