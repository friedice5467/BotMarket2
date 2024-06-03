using BotMarket2.BAL.Services.StockData;
using BotMarket2.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BotMarket2.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockDataController : ControllerBase
    {
        private readonly IStockDataService _stockDataService;
        public StockDataController(IStockDataService stockDataService)
        {
            _stockDataService = stockDataService;
        }

        [HttpGet("{symbol}")]
        public IActionResult GetStockData(string symbol)
        {
            var stockData = _stockDataService.GetStockData(symbol);
            var dtos = stockData.Select(x => ModelConvertor.StockDataToDTO(x));
            return Ok(dtos);
        }

        [HttpGet("{symbol}/{yrsFromEndDate}")]
        public IActionResult GetStockData(string symbol, int yrsFromEndDate)
        {
            var stockData = _stockDataService.GetStockData(symbol, yrsFromEndDate);
            return Ok(stockData);
        }
    }
}
