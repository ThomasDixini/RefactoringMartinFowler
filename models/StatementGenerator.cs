using System.Globalization;
using System.Text.Json;

namespace RefatoringMartinFowler.models
{
    public class StatementGenerator
    {
        public Dictionary<string, Play> Plays { get; set; } = new Dictionary<string, Play>();
        public Invoice Invoice { get; set; }
        public StatementGenerator()
        {
            var playsJson = File.ReadAllText(@"C:\Projetos\RefatoringMartinFowler\jsons\Plays.json");
            var plays = JsonSerializer.Deserialize<Dictionary<string, Play>>(playsJson) ?? throw new Exception("Plays deserialization resulted in null");

            var invoiceJson = File.ReadAllText(@"C:\Projetos\RefatoringMartinFowler\jsons\Invoices.json");
            var invoices = JsonSerializer.Deserialize<List<Invoice>>(invoiceJson) ?? throw new Exception("Invoices deserialization resulted in null");

            Plays = plays;
            Invoice = invoices.First();
        }
        public string Statement(Invoice invoice, Dictionary<string, Play> plays)
        {
            int totalAmount = 0;
            string result = $"Statement for {invoice.Customer}\n";

            foreach(var perf in invoice.Performances)
            {
                // print line for this order
                result += $"{PlayFor(perf).Name}: {Usd(AmountFor(perf))} ({perf.Audience} seats)\n";
                totalAmount += AmountFor(perf);
            }

            decimal volumeCredits = TotalAmountFor();

            result += $"Amount owed is {Usd(totalAmount)}\n";
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
            decimal result = 0m;
            result += Math.Max(aPerformance.Audience - 30, 0);
            if("comedy" == PlayFor(aPerformance).Type) result += Math.Floor((decimal) aPerformance.Audience / 5);
            return result;
        }

        public string Usd(decimal amount)
        {
            return (amount / 100).ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }

        public decimal TotalAmountFor()
        {
            decimal volumeCredits = 0m;
            foreach(var perf in Invoice.Performances)
            {
                volumeCredits += VolumeCreditsFor(perf);
            }
            return volumeCredits;
        }
    }
}