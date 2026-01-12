using System.Globalization;

namespace RefatoringMartinFowler.models
{
    public class StatementGenerator
    {
        public string Statement(Invoice invoice, Dictionary<string, Play> plays)
        {
            int totalAmount = 0;
            decimal volumeCredits = 0m;
            string result = $"Statement for {invoice.Customer}\n";
            Func<decimal, string> format = amount => amount.ToString("C", CultureInfo.GetCultureInfo("en-US"));

            foreach(var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                int thisAmount = AmountFor(perf, play);

                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if("comedy" == play.Type) volumeCredits += Math.Floor((decimal) perf.Audience / 5);

                // print line for this order
                result += $"{play.Name}: {format(thisAmount / 100)} ({perf.Audience} seats)\n";
                totalAmount += thisAmount;
            }

            result += $"Amount owed is {format(totalAmount / 100)}\n";
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }

        public int AmountFor(Performance aPerformance, Play play)
        {
            int result;
            switch(play.Type)
            {
                case "tragedy":
                    result = 40000;
                    if(aPerformance.Audience > 30)
                    {
                        result += 1000 * (aPerformance.Audience - 30);
                    }
                    break;
                case "comedy":
                    result = 30000;
                    if(aPerformance.Audience > 20)
                    {
                        result += 10000 + 500 * (aPerformance.Audience - 20);
                    }
                    result += 300 * aPerformance.Audience;
                    break;
                default:
                    throw new Exception($"unknown type: {play.Type}");
            }

            return result;
        }
    }
}