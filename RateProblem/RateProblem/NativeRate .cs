using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateProblem
{
    internal class NativeRate
    {
        public DateTime Time { get; set; }
        public string Symbol { get; set; }  // название инструмента, например EURUSD
        public double Bid { get; set; }     // это цена для покупок 
        public double Ask { get; set; }     // это цена для продаж

    }
}
