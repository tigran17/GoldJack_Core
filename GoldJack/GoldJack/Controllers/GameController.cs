﻿using System;
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
                   return BadRequest("Bet value is invalid");
                }

                var result = await _gameService.StartGame(model);
                return Ok(result);
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
            
        }

        [HttpGet("getgame")]
        public async Task<ActionResult> GetGame()
        {
            var userId = 1;
            try
            {
                var result = await _gameService.GetUserById(userId);

                await _gameService.PrepareToStartNewGame(userId);

                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost("cashback")]
        public async Task<bool> CashBackAsync (GameModel model)
        {
            try
            {
                if (model.IsCashback || model.IsEnded) return false;

                var result = await _gameService.CashBack(model);
                return result;
            }
            catch(Exception e)
            {
                return false;
            }
            
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
                return StatusCode(500, e.Message);
            }
        }


    }
}