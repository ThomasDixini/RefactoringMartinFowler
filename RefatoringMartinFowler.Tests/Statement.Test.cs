using Xunit;
using FluentAssertions;
using RefatoringMartinFowler.models;

namespace RefatoringMartinFowler.Tests;

public class UnitTest
{
    [Fact]
    public void Statement_ShouldMatch_TextOutput()
    {
        Invoice invoice = new TestData().Invoice();
        Dictionary<string, Play> plays = new TestData().Plays();
        StatementGenerator generator = new StatementGenerator();

        var result = generator.Statement(invoice, plays);
        var lines = result.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        lines[0].Should().Be("Statement for BigCo");
        lines[1].Should().Be("Hamlet: $650.00 (55 seats)");
        lines[2].Should().Be("As You Like It: $580.00 (35 seats)");
        lines[3].Should().Be("Othello: $500.00 (40 seats)");
        lines[4].Should().Be("Amount owed is $1,730.00");
        lines[5].Should().Be("You earned 47 credits");
    }
}
