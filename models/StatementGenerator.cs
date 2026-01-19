using System.Globalization;
using System.Text.Json;

namespace RefatoringMartinFowler.models
{
    public class StatementGenerator
    {
        public Dictionary<string, Play> Plays { get; set; } = new Dictionary<string, Play>();
        public StatementGenerator()
        {
            var playsJson = File.ReadAllText(@"C:\Projetos\RefatoringMartinFowler\jsons\Plays.json");
            var plays = JsonSerializer.Deserialize<Dictionary<string, Play>>(playsJson) ?? throw new Exception("Plays deserialization resulted in null");
            Plays = plays;
        }
        public string Statement(Invoice invoice, Dictionary<string, Play> plays)
        {
            int totalAmount = 0;
            decimal volumeCredits = 0m;
            string result = $"Statement for {invoice.Customer}\n";
            Func<decimal, string> format = amount => amount.ToString("C", CultureInfo.GetCultureInfo("en-US"));

            foreach(var perf in invoice.Performances)
            {
                volumeCredits += VolumeCreditsFor(perf);

                // print line for this order
                result += $"{PlayFor(perf).Name}: {format(AmountFor(perf) / 100)} ({perf.Audience} seats)\n";
                totalAmount += AmountFor(perf);
            }

            result += $"Amount owed is {format(totalAmount / 100)}\n";
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }

        public int AmountFor(Performance aPerformance)
        {
            int result;
            switch(PlayFor(aPerformance).Type)
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
                    throw new Exception($"unknown type: {PlayFor(aPerformance).Type}");
            }

            return result;
        }

        public Play PlayFor(Performance performance)
        {
            return Plays[performance.PlayID];
        }

        public decimal VolumeCreditsFor(Performance aPerformance)
        {
            decimal volumeCredits = 0m;
            volumeCredits += Math.Max(aPerformance.Audience - 30, 0);
            if("comedy" == PlayFor(aPerformance).Type) volumeCredits += Math.Floor((decimal) aPerformance.Audience / 5);
            return volumeCredits;
        }
    }
}