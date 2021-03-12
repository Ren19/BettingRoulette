using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Model
{
    public class BetInitial
    {
        public string RouletteId { get; set; }
        [Range(0, 38)]
        public int Number { get; set; }
        [Range(0, maximum: 10000)]
        public double Amount { get; set; }
        public bool IsColor { get; set; }
    }
}
