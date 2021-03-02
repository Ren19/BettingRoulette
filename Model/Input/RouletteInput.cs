using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Model.Input
{
    public class RouletteInput
    {
        public string Id { get; set; }
        public bool Open { get; set; } = false;
        public DateTime? OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
