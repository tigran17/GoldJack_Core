using AutoMapper;
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
        private IGameRepository _gameRepository;

        public GameService (IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }
        public async Task<GameModel> GetGame(GameModel model)
        {
            //TODO: Should initialize User
            model.UserId = 6; //HARD CODE

            var gameEntity = Mapper.Map<Game>(model);

            await _gameRepository.GetGame(gameEntity);

            return model;
        }

        public async Task<GameModel> StartGame(GameModel model)
        {
            var gameEntity = new Game();
            

            //TODO:Check User Balance
            //TODO: Should init User
            gameEntity.UserId = 6;  //Hard Code
            gameEntity.Coins = CreateCoins();
            ShuffleCoins(gameEntity.Coins);
            gameEntity.Range = GetRange();

            gameEntity = await _gameRepository.StartGame(gameEntity);

            var gameModel = Mapper.Map<Game, GameModel>(gameEntity);

            return gameModel;
        }

        public async Task<CoinModel> GetCoinByPosition(CoinModel model)
        {
            var coinEntity = Mapper.Map<Coin>(model);

            coinEntity = await _gameRepository.GetCoinByPosition(coinEntity);

            if (coinEntity == null) return null;

            model = Mapper.Map<Coin, CoinModel>(coinEntity);

            return model;
        }

        //private functions

        #region 

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

        private string GetRange()
        {
            Random random = new Random();
            int randomNumber = random.Next(GameConstants.MinNumber, GameConstants.MaxNumber);
            string range = String.Format("{0} - {1}", randomNumber, randomNumber + GameConstants.Range);

            return range;
        }

        //private bool CheckGameResult(CoinModel model)
        //{

        //}

        #endregion
    }
}
