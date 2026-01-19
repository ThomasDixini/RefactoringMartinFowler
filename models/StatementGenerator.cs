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
            return RenderPlainText(CreateStatementData(invoice, plays));
        }

        public string HTMLStatement(Invoice invoice, Dictionary<string, Play> plays)
        {
            return RenderHTML(CreateStatementData(invoice, plays));
        }

        public string RenderHTML(StatementData data)
        {
            var result = @$"<h1>Statement for {data.Customer}</h1>";
            result += "<table>\n";
            result += "<tr><th>Play</th><th>Seats</th><th>Cost</th></tr>";
            foreach(var perf in data.Performances)
            {
                result += $"<tr><td>{perf.Play.Name}</td><td>{perf.Audience}</td><td>{Usd(perf.Amount)}</td></tr>\n";
                result += $"<td>{Usd(perf.Amount)}</td>\n";
            }
            result += "</table>\n";
            result += $"<p>Amount owed is <em>{Usd(data.TotalAmount)}</em></p>\n";
            result += $"<p>You earned <em>{data.TotalVolumeCredits}</em> credits</p>\n";
            return result;
        }

        public StatementData CreateStatementData(Invoice invoice, Dictionary<string, Play> plays)
        {
            StatementData.Customer = invoice.Customer;
            StatementData.Performances = invoice.Performances.Select(p => EnrichPerformance(p)).ToList();
            StatementData.TotalAmount = TotalAmount(StatementData);
            StatementData.TotalVolumeCredits = TotalVolumeCredits(StatementData);
            return StatementData;
        }

        public Performance EnrichPerformance(Performance aPerformance)
        {
            var calculator = CreatePerformanceCalculator(aPerformance, PlayFor(aPerformance));
            var result = aPerformance;
            result.Play = calculator.Play;
            result.Amount = calculator.Amount;
            result.VolumeCredits = calculator.VolumeCredits;
            return result;
        }

        public PerformanceCalculator CreatePerformanceCalculator(Performance aPerformance, Play play)
        {
            switch(play.Type)
            {
                case "tragedy":
                    return new TragedyCalculator(aPerformance, play);
                case "comedy":
                    return new ComedyCalculator(aPerformance, play);
                default:
                    throw new Exception($"unknown type: {play.Type}");
            }
        }

        public string RenderPlainText(StatementData data)
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

        public Play PlayFor(Performance performance)
        {
            return Plays[performance.PlayID];
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