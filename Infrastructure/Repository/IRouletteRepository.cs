using BettingRoulette.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Repository
{
    public interface IRouletteRepository
    {
        public Roulette GetById(SearchRoulette model);
        public List<Roulette> GetAll();
        public Roulette Save(Roulette roulette);
    }
}
