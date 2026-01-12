using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RefatoringMartinFowler.models;

namespace RefatoringMartinFowler.Tests
{
    public class TestData
    {
        public Invoice Invoice() => new Invoice
        {
            Customer = "BigCo",
            Performances = new List<Performance>
            {
                new Performance { PlayID = "hamlet", Audience = 55 },
                new Performance { PlayID = "as-like", Audience = 35 },
                new Performance { PlayID = "othello", Audience = 40 },
            }
        };

        public Dictionary<string, Play> Plays() => new Dictionary<string, Play>
        {
            { "hamlet", new Play { Name = "Hamlet", Type = "tragedy" } },
            { "as-like", new Play { Name = "As You Like It", Type = "comedy" } },
            { "othello", new Play { Name = "Othello", Type = "tragedy" } },
        };
    }
}