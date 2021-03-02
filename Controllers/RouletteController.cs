using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BettingRoulette.Infrastructure.Services;
using BettingRoulette.Model;
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

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            return Ok(_rouletteService.GetAll());
        }

        [HttpPost]
        [Route("newRoulette")]
        public IActionResult NewRoulette()
        {
            return Ok(_rouletteService.Create());
        }

        [HttpPut("openRoulette")]
        public IActionResult OpenRoulette([FromBody] OpenRoulette request)
        {
            try
            {
                _rouletteService.Open(request);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(400);
            }
        }
    }
}
