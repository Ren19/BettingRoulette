using BettingRoulette.Common;
using BettingRoulette.Model;
using EasyCaching.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingRoulette.Infrastructure.Repository
{
    public class RouletteRepository : IRouletteRepository
    {
        private IEasyCachingProvider _cachingProvider;
        public RouletteRepository(IEasyCachingProvider provider)
        {
            _cachingProvider = provider ?? throw new ArgumentNullException(nameof(provider));
        }
        public List<Roulette> GetAll()
        {
            var listRoulette = _cachingProvider.GetByPrefix<Roulette>(Constants.REDIS_KEY_ROULETTE);
            if (listRoulette.Values.Count == 0)
                return new List<Roulette>();

            return new List<Roulette>(listRoulette.Select(x => x.Value.Value));
        }
        public Roulette GetById(SearchRoulette model)
        {
            var item = _cachingProvider.Get<Roulette>($"{Constants.REDIS_KEY_ROULETTE}{model.Id}");
            if (!item.HasValue)
                return null;

            return item.Value;
        }
        public Roulette Save(Roulette roulette)
        {
            var rouletteExecute = new Roulette() { Id = roulette.Id, Open = roulette.Open, OpeningDate = roulette.OpeningDate, ClosingDate = roulette.ClosingDate, ListRouletteBetDetail = roulette.ListRouletteBetDetail };
            _cachingProvider.Set($"{Constants.REDIS_KEY_ROULETTE}{rouletteExecute.Id}", rouletteExecute, TimeSpan.FromDays(365));
            return rouletteExecute;
        }
    }
}
