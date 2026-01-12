using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public class Performance
    {
        public required string PlayID { get; set; }
        public int Audience { get; set; }
    }
}