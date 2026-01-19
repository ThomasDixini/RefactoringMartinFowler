using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public class PerformanceCalculator
    {
        public Performance Performance { get; set; }
        public PerformanceCalculator(Performance aPerformance)
        {
            this.Performance = aPerformance;
        }
    }
}