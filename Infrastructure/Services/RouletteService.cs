using BettingRoulette.Infrastructure.Repository;
using BettingRoulette.Model.Input;
using BettingRoulette.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Services
{
    public class RouletteService : IRouletteService
    {
        private IRouletteRepository _rouletteRepository;
        public RouletteService(IRouletteRepository rouletteRepository) {
            _rouletteRepository = rouletteRepository ?? throw new ArgumentNullException(nameof(rouletteRepository));
        }
        public RouletteOutput Create()
        {
            RouletteInput roulette = new RouletteInput()
            {
                Id = Guid.NewGuid().ToString(),
                Open = false,
                OpeningDate = null,
                ClosingDate = null
            };
            _rouletteRepository.Save(roulette);

            return roulette;
        }
    }
}
