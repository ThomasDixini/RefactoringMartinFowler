using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefatoringMartinFowler.models
{
    public class TragedyCalculator : PerformanceCalculator
    {
        public override int Amount
        {
            get {
                int result = 40000;
                if(Performance.Audience > 30)
                {
                    result += 1000 * (Performance.Audience - 30);
                }
                return result;
            }
        }
        public TragedyCalculator(Performance aPerformance, Play play) : base(aPerformance, play)
        {
        }
    }
}