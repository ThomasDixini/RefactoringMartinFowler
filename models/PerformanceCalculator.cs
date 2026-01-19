using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public abstract class PerformanceCalculator
    {
        public Performance Performance { get; set; }
        public Play Play { get; set;}
        public abstract int Amount { get; }
        public virtual decimal VolumeCredits
        {
            get => Math.Max(Performance.Audience - 30, 0);
        }
        public PerformanceCalculator(Performance aPerformance, Play play)
        {
            this.Performance = aPerformance;
            this.Play = play;
        }
    }
}