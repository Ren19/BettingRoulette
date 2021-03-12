using BettingRoulette.Common;
using BettingRoulette.Common.GenericClass;
using BettingRoulette.Infrastructure.Exceptions;
using BettingRoulette.Infrastructure.Interfaces;
using BettingRoulette.Infrastructure.Repository;
using BettingRoulette.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BettingRoulette.Common.Enums;
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
            if (ValidationGeneral(new Validate() { ValidationType = Convert.ToInt32(Validation.Bet), Bet = model }))
            {
                Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.RouletteId });
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
            else throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_007, Message = Constants.MESSAGE_ERROR_007 }); ;
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
            if (ValidationGeneral(new Validate() { ValidationType = Convert.ToInt32(Validation.Search), SearchRoulette = model }))
                return _rouletteRepository.GetById(model);
            else throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_007, Message = Constants.MESSAGE_ERROR_007 }); ;
        }
        public List<Roulette> GetAll()
        {
            return _rouletteRepository.GetAll();
        }
        public Roulette Open(OpenRoulette model)
        {
            if (ValidationGeneral(new Validate() { ValidationType = Convert.ToInt32(Validation.Open), OpenRoulette = model }))
            {
                Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.Id });
                roulette.Open = true;
                roulette.OpeningDate = _dateFormat.GetDate();

                return _rouletteRepository.Save(roulette);
            }
            else throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_007, Message = Constants.MESSAGE_ERROR_007 }); ;
        }
        public Roulette Close(CloseRoulette model)
        {
            if (ValidationGeneral(new Validate() { ValidationType = Convert.ToInt32(Validation.Close), CloseRoulette = model }))
            {
                Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = model.Id });
                var numberWiiner = GetWinningNumber(roulette);
                roulette = AssignWinner(new RouletteWinner() { NumberWinner = numberWiiner, Roulette = roulette });
                roulette.ClosingDate = _dateFormat.GetDate();
                roulette.Open = false;

                return _rouletteRepository.Save(roulette);
            }
            else throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_007, Message = Constants.MESSAGE_ERROR_007 }); ;
        }
        #region Select Winner
        public int GetWinningNumber(Roulette roulette)
        {
            Random randon = new Random();
            var winner = 0;
            bool winnerRandom = false;
            roulette.ListRouletteBetDetail.RemoveAll(x => string.IsNullOrEmpty(x.Player));
            while (!winnerRandom)
            {
                winner = randon.Next(Constants.MIN_NUMBER_BET, Constants.MAX_NUMBER_BET);
                if (roulette.ListRouletteBetDetail.Where(x => x.Number == winner).Count() > 0)
                    winnerRandom = true;
            }
            return winner;
        }
        public Roulette AssignWinner(RouletteWinner rouletteWinner)
        {
            foreach (var item in rouletteWinner.Roulette.ListRouletteBetDetail)
            {
                if (item.Number == rouletteWinner.NumberWinner && !item.IsColor && !string.IsNullOrEmpty(item.Player))
                { item.Winner = true; item.PrizeAmount = item.AmountBet * Constants.NUMBER_BET_PAYAOUT; }
                else if (item.Number == rouletteWinner.NumberWinner && item.IsColor && !string.IsNullOrEmpty(item.Player))
                { item.Winner = true; item.PrizeAmount = item.AmountBet * Constants.COLOR_BET_PAYAOUT; }
                else
                { item.Winner = false; item.PrizeAmount = item.AmountBet * 0; }
            }
            return rouletteWinner.Roulette;
        }
        #endregion
        #region Validations
        public bool ValidationGeneral(Validate validate)
        {
            if (validate == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_005, Message = Constants.MESSAGE_ERROR_005 });
            switch (validate.ValidationType)
            {
                case 1: return ValidationSearch(validate.SearchRoulette);
                case 2: return ValidationOpen(validate.OpenRoulette);
                case 3: return ValidationClose(validate.CloseRoulette);
                case 4: return ValidationBet(validate.Bet);
                default: return false;
            }
        }
        public bool ValidationSearch(SearchRoulette searchRoulette)
        {
            if (searchRoulette == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_005, Message = Constants.MESSAGE_ERROR_005 });
            if (searchRoulette.Id.ToString().Equals(""))
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_006, Message = Constants.MESSAGE_ERROR_006 });

            return true;
        }
        public bool ValidationOpen(OpenRoulette openRoulette)
        {
            if (openRoulette == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_005, Message = Constants.MESSAGE_ERROR_005 });
            if (openRoulette.Id.ToString().Equals(""))
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_006, Message = Constants.MESSAGE_ERROR_006 });
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = openRoulette.Id });
            if (roulette == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_001, Message = Constants.MESSAGE_ERROR_001 });
            if (roulette.Open)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_002, Message = Constants.MESSAGE_ERROR_002 });
            
            return true;
        }
        public bool ValidationClose(CloseRoulette closeRoulette)
        {
            if (closeRoulette == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_005, Message = Constants.MESSAGE_ERROR_005 });
            if (closeRoulette.Id.ToString().Equals(""))
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_006, Message = Constants.MESSAGE_ERROR_006 });
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = closeRoulette.Id });
            if (roulette == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_001, Message = Constants.MESSAGE_ERROR_001 });
            if (!roulette.Open)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_003, Message = Constants.MESSAGE_ERROR_003 });

            return true;
        }
        public bool ValidationBet(Bet bet)
        {
            if (bet == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_005, Message = Constants.MESSAGE_ERROR_005 });
            if (bet.RouletteId.ToString().Equals(""))
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_006, Message = Constants.MESSAGE_ERROR_006 });
            if (bet.Amount < Constants.MIN_AMOUNT_BET || bet.Amount > Constants.MAX_AMOUNT_BET)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_004, Message = Constants.MESSAGE_ERROR_004 });
            Roulette roulette = _rouletteRepository.GetById(new SearchRoulette() { Id = bet.RouletteId });
            if (roulette == null)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_001, Message = Constants.MESSAGE_ERROR_001 });
            if (!roulette.Open)
                throw new RouletteException(400, new ServiceResponse { Code = Constants.CODE_ERROR_003, Message = Constants.MESSAGE_ERROR_003 });

            return true;
        }
        #endregion

    }
}
