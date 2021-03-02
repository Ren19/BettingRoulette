using BettingRoulette.Common;
using BettingRoulette.Infrastructure.Repository;
using BettingRoulette.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Services
{
    public class RouletteService : IRouletteService
    {
        private IRouletteRepository _rouletteRepository;
        private IDateFormat _dateFormat;
        public RouletteService(IRouletteRepository rouletteRepository, IDateFormat dateFormat) {
            _rouletteRepository = rouletteRepository ?? throw new ArgumentNullException(nameof(rouletteRepository));
            _dateFormat = dateFormat ?? throw new ArgumentNullException(nameof(dateFormat));
        }
        public Roulette Bet(Bet request, string UserId)
        {
            throw new NotImplementedException();
        }
        public string Create()
        {
            Roulette roulette = new Roulette()
            {
                Id = Guid.NewGuid().ToString(),
                Open = false,
                OpeningDate = null,
                ClosingDate = null
            };
            var execute = _rouletteRepository.Save(roulette);
            
            return execute.Id;
        }
        public Roulette Search(SearchRoulette model)
        {
            return _rouletteRepository.GetById(model);
        }
        public List<Roulette> GetAll()
        {
            return _rouletteRepository.GetAll();
        }
        public Roulette Open(OpenRoulette model)
        {
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.Id });
            roulette.Open = true;
            roulette.OpeningDate = _dateFormat.GetDate();

            return _rouletteRepository.Save(roulette);
        }
        public Roulette Close(CloseRoulette model)
        {
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.Id });
            Random randon = new Random();
            var winner = randon.Next(0, 38);
            //foreach (var item in roulette.numberBets)
            //{
            //    if (item.Number == winner && !item.IsColor && !string.IsNullOrEmpty(item.Player))
            //    {
            //        item.IsWinningNumber = true;
            //        item.TotalWon = item.AmountBet * Constants.NUMBER_BET_PAYAOUT;
            //    }
            //    else if (item.Number == winner && item.IsColor && !string.IsNullOrEmpty(item.Player))
            //    {
            //        item.IsWinningNumber = true;
            //        item.TotalWon = item.AmountBet * Constants.COLOR_BET_PAYAOUT;
            //    }
            //    else
            //    {
            //        item.IsWinningNumber = false;
            //        item.TotalWon = item.AmountBet * 0;
            //    }
            //}
            roulette.ClosingDate = _dateFormat.GetDate();
            roulette.Open = false;

            return _rouletteRepository.Save(roulette);
        }
    }
}
