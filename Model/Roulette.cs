using BettingRoulette.Common;
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
            ListRouletteBetDetail = new List<RouletteBetDetail>();
            for (int i = Constants.MIN_NUMBER_BET; i < Constants.MAX_NUMBER_BET; i++)
            {
                ListRouletteBetDetail.Add(new RouletteBetDetail()
                {
                    Number = i,
                    Player = null,
                    AmountBet = null,
                    PrizeAmount = null,
                    Winner = false,
                    IsColor = false
                });
            }
        }
        public string Id { get; set; }
        public bool Open { get; set; } = false;
        public DateTime? OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public List<RouletteBetDetail> ListRouletteBetDetail { get; set; }
    }
}
