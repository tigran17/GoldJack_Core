﻿using AutoMapper;
using GoldJack.Constants;
using GoldJack.Entities;
using GoldJack.Interfaces.DAL;
using GoldJack.Interfaces.Services;
using GoldJack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldJack.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public GameService (IGameRepository gameRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }
        public async Task<GameModel> GetGame()
        {
            //TODO: Should initialize User
            var userId = 6; //HARD CODE

            //var gameEntity = _mapper.Map<Game>(model);

            var gameEntity = await _gameRepository.GetUserLastGame(userId);

            gameEntity.Coins =  await _gameRepository.GetGameOpenedCoins(gameEntity.Id);

            var model = _mapper.Map<GameModel>(gameEntity);

            return model;
        }

        public async Task<bool> CashBack(GameModel model)
        {
            var gameEntity = _mapper.Map<Game>(model);

            var game = await _gameRepository.GetGameById(gameEntity.Id);

            if (game == null) return false;

            if (!CheckForCashback(game)) return false;

            game.IsCashback = true;
            game.IsEnded = true;

            //gameEntity Cashback to  User balance

            var result = await _gameRepository.UpdateGame(game);

            return result;
        }

        public async Task<GameModel> StartGame(GameModel model)
        {
            //TODO:Check User Balance
            //TODO: Should init User
            model.UserId = 6;  //Hard Code
            SetRange(model);

            var gameEntity = _mapper.Map<GameModel, Game>(model);

            gameEntity.Coins = CreateCoins();

            ShuffleCoins(gameEntity.Coins);

            gameEntity = await _gameRepository.SaveGame(gameEntity);

            var gameModel = _mapper.Map<Game, GameModel>(gameEntity);

            return gameModel;
        }

        public async Task<CoinModel> GetCoinByPosition(CoinModel model)
        {
            var coinEntity = _mapper.Map<Coin>(model);

            coinEntity = await _gameRepository.GetCoinByPosition(coinEntity);

            if (coinEntity == null) return null;

            coinEntity.IsOpened = true;

            await _gameRepository.UpdateCoin(coinEntity);

            var gameEntity = await _gameRepository.GetGameById(model.GameId);

            gameEntity = IsGameEnded(gameEntity);

            await _gameRepository.UpdateGame(gameEntity);

            //Return only requsted coin 
            gameEntity.Coins = null;

            coinEntity.Game = gameEntity;

            var coinModel = _mapper.Map<Coin, CoinModel>(coinEntity);

            return coinModel;
        }


        #region private functions

        private bool CheckForCashback(Game game)
        {
            var coinsSum = game.Coins.Where(x => x.IsOpened).Select(x => x.Value).Sum();

            if(coinsSum >= game.StartRange && coinsSum <= game.EndRange)
            {
                return true;
            }

            return false;
        }

        private Game IsGameEnded(Game game)
        {
            var openCoins = game.Coins.Where(x => x.IsOpened);
            var coinsSum = game.Coins.Where(x => x.IsOpened).Select(x => x.Value).Sum();

            if (openCoins.Count() == GameConstants.OpenedCoinsMaxCount)
            {
                game.IsEnded = true;

                if (coinsSum <= game.EndRange)
                {
                    
                    game.IsWin = true;
                }
                else
                {
                    game.IsWin = false;
                }
            }
            else
            {
                if(coinsSum >= game.EndRange)
                {
                    game.IsEnded = true;
                    game.IsWin = false;
                }
            }

            return game;
        }

        private List<Coin> CreateCoins()
        {
            List<Coin> coins = new List<Coin>();

            for (int i = 0; i < GameConstants.CoinCount; i++)
            {
                coins.Add(new Coin() { Value = i + 1, IsOpened = false });
            }

            return coins;
        }

        private void ShuffleCoins(List<Coin> coins)
        {
            Random rnd = new Random();
            coins = coins.OrderBy(x => rnd.Next()).ToList();

            int i = 0;
            foreach (var item in coins)
            {
                item.Position = ++i;
            }
        }

        private void SetRange(GameModel gameModel)
        {
            Random random = new Random();
            short randomNumber = (short)random.Next(GameConstants.MinNumber, GameConstants.MaxNumber);
            //string range = String.Format("{0} - {1}", randomNumber, randomNumber + GameConstants.Range);
            gameModel.StartRange = randomNumber;
            gameModel.EndRange = (short)(randomNumber + GameConstants.Range);
        }

        #endregion
    }
}
