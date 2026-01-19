using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public class ComedyCalculator : PerformanceCalculator
    {
        public override int Amount
        {
            get {
                int result = 30000;
                if(Performance.Audience > 20)
                {
                    result += 10000 + 500 * (Performance.Audience - 20);
                }
                result += 300 * Performance.Audience;
                return result;
            }
        }

        public decimal VolumeCredits
        {
            get => base.VolumeCredits + Math.Floor((decimal) Performance.Audience / 5);
        }
        public ComedyCalculator(Performance aPerformance, Play play) : base(aPerformance, play)
        {
        }
    }
}