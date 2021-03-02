using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingRoulette.Infrastructure.Services;
using BettingRoulette.Model.Output;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BettingRoulette.Controllers
{
    [Route("api/masivian/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {
        IRouletteService _rouletteService;
        public RouletteController(IRouletteService rouletteService)
        {
            _rouletteService = rouletteService ?? throw new ArgumentNullException(nameof(rouletteService));
        }

        [HttpPost]
        public IActionResult NewRoulette()
        {
            RouletteOutput roulette = _rouletteService.Create();
            return Ok(roulette);
        }
    }
}
