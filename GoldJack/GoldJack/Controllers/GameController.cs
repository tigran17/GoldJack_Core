using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldJack.Interfaces.Services;
using GoldJack.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoldJack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("startgame")]
        public async Task<ActionResult<GameModel>> StartGame([FromBody]GameModel model)
        {
            try
            {
                if(model.Bet <= 0)
                {
                    throw new Exception("Bet value is invalid");
                }

                var result = await _gameService.StartGame(model);
                return result;
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(e); 
            }
            
        }

        [HttpGet]
        public ActionResult<GameModel> GetGame()
        {
            return new GameModel();
        }

        [HttpGet]
        public ActionResult<bool> CashBack ()
        {
            return true;
        }

        [HttpPost("getcoin")]
        public async Task<ActionResult<CoinModel>> GetCoinByPosition([FromBody]CoinModel model)
        {
            try
            {
                var result = await _gameService.GetCoinByPosition(model);

                return result;
            }
            catch(Exception e)
            {
                throw e;
            }
        }


    }
}