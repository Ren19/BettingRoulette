using BettingRoulette.Model.Input;
using BettingRoulette.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Services
{
    public interface IRouletteService
    {
        RouletteOutput Create();
        /*
        RouletteOutput Open(string Id);
        RouletteOutput Find(string Id);
        RouletteOutput Close(string Id);
        RouletteOutput Bet(BetInput request, string UserId);
        List<RouletteOutput> GetAll();
        */
    }
}
