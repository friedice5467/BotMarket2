using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BotMarket2.Common.Models
{
    public class HistoricalStockData
    {
        public int HistoricalStockDataId { get; set; }
        public string? Symbol { get; set; }
        public DateTime Date { get; set; }
        [Column("CloseLast")]
        [Precision(18, 2)]
        public decimal CloseLast { get; set; }
        public long Volume { get; set; }
        [Precision(18, 2)]
        public decimal Open { get; set; }
        [Precision(18, 2)]
        public decimal High { get; set; }
        [Precision(18, 2)]
        public decimal Low { get; set; }
    }

}
