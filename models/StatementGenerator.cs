using System.Globalization;
using System.Text.Json;

namespace RefatoringMartinFowler.models
{
    public class StatementGenerator
    {
        public Dictionary<string, Play> Plays { get; set; } = new Dictionary<string, Play>();
        public Invoice Invoice { get; set; }
        StatementData StatementData = new StatementData();
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
            StatementData.Customer = invoice.Customer;
            StatementData.Performances = invoice.Performances.Select(p => EnrichPerformance(p)).ToList();
            StatementData.TotalAmount = TotalAmount(StatementData);
            StatementData.TotalVolumeCredits = TotalVolumeCredits(StatementData);
            return RenderPlainText(StatementData, invoice, plays);
        }

        public Performance EnrichPerformance(Performance aPerformance)
        {
            var result = aPerformance;
            result.Play = PlayFor(aPerformance);
            result.Amount = AmountFor(result);
            result.VolumeCredits = VolumeCreditsFor(result);
            return result;
        }

        public string RenderPlainText(StatementData data, Invoice invoice, Dictionary<string, Play> plays)
        {
            string result = $"Statement for {data.Customer}\n";
            foreach(var perf in data.Performances)
            {
                result += $"{perf.Play.Name}: {Usd(perf.Amount)} ({perf.Audience} seats)\n";
            }

            result += $"Amount owed is {Usd(data.TotalAmount)}\n";
            result += $"You earned {data.TotalVolumeCredits} credits\n";
            return result;
        }

        public int AmountFor(Performance aPerformance)
        {
            int result;
            switch(aPerformance.Play.Type)
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
                    throw new Exception($"unknown type: {aPerformance.Play.Type}");
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
            if("comedy" == aPerformance.Play.Type) result += Math.Floor((decimal) aPerformance.Audience / 5);
            return result;
        }

        public string Usd(decimal amount)
        {
            return (amount / 100).ToString("C", CultureInfo.GetCultureInfo("en-US"));
        }

        public decimal TotalVolumeCredits(StatementData data)
        {
            return data.Performances.Sum(p => p.VolumeCredits);
        }

        public int TotalAmount(StatementData data)
        {
            return data.Performances.Sum(p => p.Amount);
        }
    }
}