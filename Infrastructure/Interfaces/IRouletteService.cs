using BettingRoulette.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Interfaces
{
    public interface IRouletteService
    {
        string Create();
        List<Roulette> GetAll();
        Roulette Open(OpenRoulette model);
        Roulette Search(SearchRoulette model);
        Roulette Close(CloseRoulette model);
        Roulette Bet(Bet model);
    }
}
