using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public class StatementData
    {
        public string Customer { get; set; }
        public List<Performance> Performances { get; set; } = new List<Performance>();
        public int TotalAmount { get; set; }
        public decimal TotalVolumeCredits { get; set; }
    }
}