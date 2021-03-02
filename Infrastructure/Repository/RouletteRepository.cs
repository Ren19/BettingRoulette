using BettingRoulette.Common;
using BettingRoulette.Model.Input;
using BettingRoulette.Model.Output;
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
        public List<RouletteOutput> GetAll()
        {
            var rouletes = _cachingProvider.GetByPrefix<RouletteOutput>(Constants.REDIS_KEY_ROULETTE);
            if (rouletes.Values.Count == 0)
            {
                return new List<RouletteOutput>();
            }

            return new List<RouletteOutput>(rouletes.Select(x => x.Value.Value));
        }
        public RouletteOutput GetById(string Id)
        {
            var item = _cachingProvider.Get<RouletteOutput>($"{Constants.REDIS_KEY_ROULETTE}{Id}");
            if (!item.HasValue)
            {
                return null;
            }
            return item.Value;
        }
        public RouletteOutput Save(RouletteInput roulette)
        {
            _cachingProvider.Set($"{Constants.REDIS_KEY_ROULETTE}{roulette.Id}", roulette, TimeSpan.FromDays(365));
            return new RouletteOutput() { Id = roulette.Id };
        }
        public RouletteOutput Update(string Id, RouletteInput roulette)
        {
            roulette.Id = Id;
            return Save(roulette);
        }
    }
}
