using BettingRoulette.Common;
using BettingRoulette.Infrastructure.Exceptions;
using BettingRoulette.Infrastructure.Interfaces;
using BettingRoulette.Infrastructure.Repository;
using BettingRoulette.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BettingRoulette.Infrastructure.Exceptions.RouletteException;

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
        public Roulette Bet(Bet model)
        {
            if (model == null)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_005,
                    Message = Constants.MESSAGE_ERROR_005
                });
            }
            if (model.RouletteId.ToString().Equals(""))
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_006,
                    Message = Constants.MESSAGE_ERROR_006
                });
            }
            if (model.Amount < Constants.MIN_AMOUNT_BET || model.Amount > Constants.MAX_AMOUNT_BET)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_004,
                    Message = Constants.MESSAGE_ERROR_004
                });
            }
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.RouletteId });
            if (roulette == null)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_001,
                    Message = Constants.MESSAGE_ERROR_001
                });
            }
            if (!roulette.Open)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_003,
                    Message = Constants.MESSAGE_ERROR_003
                });
            }
            foreach (var item in roulette.ListRouletteBetDetail)
            {
                if (item.Number == model.Number)
                {
                    item.Player = model.UserId;
                    item.AmountBet = model.Amount;
                    item.IsColor = model.IsColor;
                }
            }
            return _rouletteRepository.Save(roulette);
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
            if (model == null)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_005,
                    Message = Constants.MESSAGE_ERROR_005
                });
            }
            if (model.Id.ToString().Equals(""))
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_006,
                    Message = Constants.MESSAGE_ERROR_006
                });
            }

            return _rouletteRepository.GetById(model);
        }
        public List<Roulette> GetAll()
        {
            return _rouletteRepository.GetAll();
        }
        public Roulette Open(OpenRoulette model)
        {
            if (model == null)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_005,
                    Message = Constants.MESSAGE_ERROR_005
                });
            }
            if (model.Id.ToString().Equals(""))
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_006,
                    Message = Constants.MESSAGE_ERROR_006
                });
            }
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.Id });
            if (roulette == null)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_001,
                    Message = Constants.MESSAGE_ERROR_001
                });
            }
            if (roulette.Open)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_002,
                    Message = Constants.MESSAGE_ERROR_002
                });
            }
            roulette.Open = true;
            roulette.OpeningDate = _dateFormat.GetDate();

            return _rouletteRepository.Save(roulette);
        }
        public Roulette Close(CloseRoulette model)
        {
            Random randon = new Random();
            var winner = 0;
            bool winnerRandom = false;
            if (model == null)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_005,
                    Message = Constants.MESSAGE_ERROR_005
                });
            }
            if (model.Id.ToString().Equals(""))
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_006,
                    Message = Constants.MESSAGE_ERROR_006
                });
            }
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.Id });
            if (roulette == null)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_001,
                    Message = Constants.MESSAGE_ERROR_001
                });
            }
            if (!roulette.Open)
            {
                throw new RouletteException(400, new ServiceResponse
                {
                    Code = Constants.CODE_ERROR_003,
                    Message = Constants.MESSAGE_ERROR_003
                });
            }
            roulette.ListRouletteBetDetail.RemoveAll(x => string.IsNullOrEmpty(x.Player));
            while (!winnerRandom)
            {
                winner = randon.Next(Constants.MIN_NUMBER_BET, Constants.MAX_NUMBER_BET);

                if (roulette.ListRouletteBetDetail.Where(x => x.Number == winner).Count() > 0)
                {
                    winnerRandom = true;
                }
            }
            foreach (var item in roulette.ListRouletteBetDetail)
            {
                if (item.Number == winner && !item.IsColor && !string.IsNullOrEmpty(item.Player))
                {
                    item.Winner = true;
                    item.PrizeAmount = item.AmountBet * Constants.NUMBER_BET_PAYAOUT;
                }
                else if (item.Number == winner && item.IsColor && !string.IsNullOrEmpty(item.Player))
                {
                    item.Winner = true;
                    item.PrizeAmount = item.AmountBet * Constants.COLOR_BET_PAYAOUT;
                }
                else
                {
                    item.Winner = false;
                    item.PrizeAmount = item.AmountBet * 0;
                }
            }
            roulette.ClosingDate = _dateFormat.GetDate();
            roulette.Open = false;

            return _rouletteRepository.Save(roulette);
        }
    }
}
