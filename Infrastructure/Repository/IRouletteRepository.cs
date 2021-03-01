using BettingRoulette.Model.Input;
using BettingRoulette.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Repository
{
    public interface IRouletteRepository
    {
        public RouletteOutput GetById(string Id);
        public List<RouletteOutput> GetAll();
        public RouletteOutput Update(string Id, RouletteInput roulette);
        public RouletteOutput Save(RouletteInput roulette);
    }
}
