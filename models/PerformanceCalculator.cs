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
        public int Amount
        {
            get {
                int result;
                switch(Performance.Play.Type)
                {
                    case "tragedy":
                        result = 40000;
                        if(Performance.Audience > 30)
                        {
                            result += 1000 * (Performance.Audience - 30);
                        }
                        break;
                    case "comedy":
                        result = 30000;
                        if(Performance.Audience > 20)
                        {
                            result += 10000 + 500 * (Performance.Audience - 20);
                        }
                        result += 300 * Performance.Audience;
                        break;
                    default:
                        throw new Exception($"unknown type: {Performance.Play.Type}");
                }

                return result;
            }
        }
        public decimal VolumeCredits
        {
            get {
                decimal result = 0m;
                result += Math.Max(Performance.Audience - 30, 0);
                if("comedy" == Performance.Play.Type) result += Math.Floor((decimal) Performance.Audience / 5);
                return result;
            }
        }
        public PerformanceCalculator(Performance aPerformance, Play play)
        {
            this.Performance = aPerformance;
            this.Play = play;
        }
    }
}