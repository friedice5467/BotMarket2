using BotMarket2.Shared.DTO;

namespace BotMarket2.Client.Models.Backtest
{
    public class PortfolioManager
    {
        public decimal InitialInvestment { get; }
        private decimal cash;
        private PriorityQueue<Transaction, decimal> buyQueue = new PriorityQueue<Transaction, decimal>(); 
        private List<Transaction> ledger = new List<Transaction>(); 
        /// <summary>
        /// The target profit percentage to sell the stock
        /// </summary>
        public decimal TargetProfitPercentage { get; }
        /// <summary>
        /// The percentage of cash to use for buying a stock
        /// </summary>
        public decimal BuyPercentage { get; }

        public PortfolioManager(decimal initialInvestment, decimal targetProfitPercentage, decimal buyPercentage)
        {
            buyQueue = new PriorityQueue<Transaction, decimal>();
            ledger = new List<Transaction>();
            InitialInvestment = initialInvestment;
            cash = initialInvestment;
            TargetProfitPercentage = targetProfitPercentage;
            BuyPercentage = buyPercentage;
        }

        public void ActOnSignal(HistoricalStockDataDTO data, SignalType sType)
        {
            if (sType == SignalType.Buy)
                Buy(data);
            else if (sType == SignalType.Sell)
                Sell(data);
        }

        public void Buy(HistoricalStockDataDTO data)
        {
            decimal price = data.CloseLast;
            decimal quantity = (int)(cash * BuyPercentage / price);
            decimal totalCost = price * quantity;
            if (totalCost <= cash)
            {
                Transaction transaction = new Transaction
                {
                    Symbol = data.Symbol ?? "UnknownSymbol",
                    Price = price,
                    Quantity = quantity,
                    TransactionType = TransactionType.Buy,
                    TransactionDate = data.Date
                };
                buyQueue.Enqueue(transaction, price);
                ledger.Add(transaction);
                cash -= totalCost;
            }
            else
            {
                throw new InvalidOperationException("Not enough cash to complete the transaction.");
            }
        }

        public void Sell(HistoricalStockDataDTO data)
        {
            if (buyQueue.Count == 0) return;

            while (buyQueue.Count > 0 && buyQueue.TryPeek(out Transaction transaction, out _))
            {
                decimal currentPrice = data.CloseLast;
                decimal profitRatio = (currentPrice - transaction.Price) / transaction.Price;

                if (profitRatio >= TargetProfitPercentage)
                {
                    buyQueue.Dequeue(); 
                    Transaction sellTransaction = new Transaction
                    {
                        Symbol = transaction.Symbol,
                        Price = currentPrice,
                        Quantity = transaction.Quantity,
                        TransactionType = TransactionType.Sell,
                        TransactionDate = data.Date
                    };
                    ledger.Add(sellTransaction);
                    cash += currentPrice * transaction.Quantity;
                }
                else
                {
                    break;
                }
            }
        }

        public List<Transaction> GetLedger()
        {
            return ledger;
        }

        public decimal GetCurrentBalance()
        {
            return cash;
        }

        public decimal GetNetProfit(Dictionary<string, decimal> currentPrices)
        {
            decimal portfolioValue = GetPortfolioValue(currentPrices);
            return portfolioValue - InitialInvestment;
        }

        public decimal GetPortfolioValue(Dictionary<string, decimal> currentPrices)
        {
            decimal portfolioValue = cash;
            foreach (var transaction in ledger.Where(t => t.TransactionType == TransactionType.Buy))
            {
                if (currentPrices.TryGetValue(transaction.Symbol, out decimal currentPrice))
                {
                    portfolioValue += currentPrice * transaction.Quantity;
                }
            }
            return portfolioValue;
        }
    }
}
