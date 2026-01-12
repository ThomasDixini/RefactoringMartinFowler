using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public class Invoice
    {
        public required string Customer { get; set; }
        public required List<Performance> Performances { get; set; }
    }
}