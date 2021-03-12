using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Model
{
    public class Validate
    {
        public int ValidationType { get; set; }
        public SearchRoulette SearchRoulette { get; set; }
        public OpenRoulette OpenRoulette { get; set; }
        public CloseRoulette CloseRoulette { get; set; }
        public Bet Bet { get; set; }
    }
}
