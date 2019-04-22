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
        private readonly IGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public GameService (IGameRepository gameRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }
        public async Task<GameModel> GetGame(GameModel model)
        {
            //TODO: Should initialize User
            model.UserId = 6; //HARD CODE

            var gameEntity = _mapper.Map<Game>(model);

            await _gameRepository.GetGame(gameEntity);

            return model;
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

            model = _mapper.Map<Coin, CoinModel>(coinEntity);

            return model;
        }


        #region private functions

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
