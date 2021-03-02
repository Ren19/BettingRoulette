using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Model
{
    public class Bet
    {
        public string RouletteId { get; set; }
        [Range(0.1d, maximum: 10000)]
        public double Money { get; set; }
        [Range(0, 38)]
        public int Position { get; set; }
        public bool IsColor { get; set; }
    }
}
