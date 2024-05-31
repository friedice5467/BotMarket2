
using Microsoft.EntityFrameworkCore;

namespace BotMarket2.Common.Models
{
    public class Simulation
    {
        public int SimulationId { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Precision(18, 2)]
        public decimal InitialInvestment { get; set; }
        [Precision(18, 2)]
        public decimal FinalBalance { get; set; }
        public Algorithm Algorithm { get; set; }
        public SimulationType SimulationType { get; set; }
    }

    public enum SimulationType : int
    {
        Yearly = 1,
        Yearly5 = 2
    }

}
