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
                int thisAmount = 0;

                switch(play.Type)
                {
                    case "tragedy":
                        thisAmount = 40000;
                        if(perf.Audience > 30)
                        {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }
                        break;
                    case "comedy":
                        thisAmount = 30000;
                        if(perf.Audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }
                        thisAmount += 300 * perf.Audience;
                        break;
                    default:
                        throw new Exception($"unknown type: {play.Type}");
                }

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
    }
}