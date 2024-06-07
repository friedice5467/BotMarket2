namespace BotMarket2.Client.Models.Backtest
{
    public class SignalAggregator
    {
        public SignalResult AggregateSignals(List<SignalResult> signals, AggregationMode mode, int confirmationThreshold = 0)
        {
            switch (mode)
            {
                case AggregationMode.MajorityVote:
                    return MajorityVote(signals);
                case AggregationMode.SignalPriority:
                    return SignalPriority(signals);
                case AggregationMode.SignalConfirmation:
                    return SignalConfirmation(signals, confirmationThreshold);
                default:
                    throw new InvalidOperationException("Invalid aggregation mode.");
            }
        }

        public SignalResult MajorityVote(List<SignalResult> signals)
        {
            int buys = signals.Count(sr => sr.SignalType == SignalType.Buy);
            int sells = signals.Count(sr => sr.SignalType == SignalType.Sell);
            int holds = signals.Count(sr => sr.SignalType == SignalType.None);
            bool? isBuy = null;
            if (buys > sells && buys > holds)
                isBuy = true;
            else if (sells > buys && sells > holds)
                isBuy = false;
            return new SignalResult(isBuy, signals.First().Price, signals.First().Date, "Majority Vote", 0);
        }

        public SignalResult SignalPriority(List<SignalResult> signals)
        {
            return signals.OrderByDescending(sr => sr.StrategyPriority).First();
        }

        public SignalResult SignalConfirmation(List<SignalResult> signals, int confirmationThreshold)
        {
            int buyCount = signals.Count(sr => sr.SignalType == SignalType.Buy);
            int sellCount = signals.Count(sr => sr.SignalType == SignalType.Sell);
            if (buyCount >= confirmationThreshold)
                return new SignalResult(true, signals.First().Price, signals.First().Date, "Confirmed Buy", 0);
            if (sellCount >= confirmationThreshold)
                return new SignalResult(false, signals.First().Price, signals.First().Date, "Confirmed Sell", 0);

            return new SignalResult(null, signals.First().Price, signals.First().Date, "No Signal", 0);
        }
    }

    public enum AggregationMode
    {
        MajorityVote,
        SignalPriority,
        SignalConfirmation
    }

}
