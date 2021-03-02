using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Model
{
    public class Roulette
    {
        public Roulette()
        {
            this.Init();
        }
        private void Init()
        {
            /*
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = new Dictionary<string, double>();
            }
            */
        }
        public string Id { get; set; }
        public bool Open { get; set; } = false;
        public DateTime? OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
