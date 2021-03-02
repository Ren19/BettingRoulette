using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Model
{
    public class RouletteBetDetail
    {
        public int Number { get; set; }
        public string Player { get; set; }
        public double? AmountBet { get; set; }
        public double? PrizeAmount { get; set; }
        public bool Winner { get; set; }
        public bool IsColor { get; set; }
    }
}
