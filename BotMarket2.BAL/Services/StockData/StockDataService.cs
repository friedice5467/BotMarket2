using BotMarket2.Client.Models.MACD;
using BotMarket2.Common.Models;
using BotMarket2.DAL.Data.Repository.StockData;
using BotMarket2.Shared;
using BotMarket2.Shared.DTO;
using Skender.Stock.Indicators;

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

        public IEnumerable<HistoricalStockDataDTO> GetStockData(string symbol, int yrsFromEndDate)
        {
            var historicalData = _stockDataRepo.GetStockData(symbol, yrsFromEndDate).ToList();
            var quotes = historicalData.Select(x => new Quote() { Date = x.Date, Open = x.Open, High = x.High, Low = x.Low, Close = x.CloseLast, Volume = x.Volume }).ToList();

            var smaResults = quotes.GetSma(14);
            var emaResults = quotes.GetEma(14);
            var rsiResults = quotes.GetRsi(14);
            var macdResults = quotes.GetMacd();
            var bbResults = quotes.GetBollingerBands(20, 2);

            var smaDictionary = smaResults.ToDictionary(x => x.Date, x => x.Sma);
            var emaDictionary = emaResults.ToDictionary(x => x.Date, x => x.Ema);
            var rsiDictionary = rsiResults.ToDictionary(x => x.Date, x => x.Rsi);
            var macdDictionary = macdResults.ToDictionary(x => x.Date, x => x);
            var bbDictionary = bbResults.ToDictionary(x => x.Date, x => x);

            List<HistoricalStockDataDTO> dtos = new();

            foreach (var data in historicalData)
            {
                HistoricalStockDataDTO dto = ModelConvertor.StockDataToDTO(data);
                dto.SMA = smaDictionary.TryGetValue(data.Date, out var sma) ? sma : null;
                dto.EMA = emaDictionary.TryGetValue(data.Date, out var ema) ? ema : null;
                dto.RSI = rsiDictionary.TryGetValue(data.Date, out var rsi) ? rsi : null;

                dto.MACD = macdDictionary.TryGetValue(data.Date, out var macd) ? MACDConvertor(macd) : null;

                if (bbDictionary.TryGetValue(data.Date, out var bb))
                {
                    dto.BollingerBandUpper = bb.UpperBand;
                    dto.BollingerBandLower = bb.LowerBand;
                }

                dtos.Add(dto);
            }

            return dtos;
        }

        private static MacdResultDTO MACDConvertor(MacdResult macdResult)
        {
            return new MacdResultDTO()
            {
                Macd = macdResult.Macd,
                Signal = macdResult.Signal,
                Histogram = macdResult.Histogram,
                FastEma = macdResult.FastEma,
                SlowEma = macdResult.SlowEma
            };
        }
    }
}
