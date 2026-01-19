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
        public Play Play { get; set; }
        public int Amount { get ; set; }
        public decimal VolumeCredits { get; set; }
    }
}