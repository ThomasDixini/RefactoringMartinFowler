using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public class PerformanceCalculator
    {
        public Performance Performance { get; set; }
        public Play Play { get; set;}
        public PerformanceCalculator(Performance aPerformance, Play play)
        {
            this.Performance = aPerformance;
            this.Play = play;
        }
    }
}