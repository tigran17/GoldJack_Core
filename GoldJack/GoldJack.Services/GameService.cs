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
        public async Task<List<GameModel>> GetGame()
        {
            var startedGames = new List<GameModel>();
            //TODO: Should initialize User
            var userId = 9; //HARD CODE
            //Bouns Game Logic
            var gameEntity = await _gameRepository.GetUserLastGame(userId);

            if (gameEntity == null || gameEntity.IsEnded) return null;

            gameEntity.Coins = await _gameRepository.GetGameOpenedCoins(gameEntity.Id);

            if (!gameEntity.IsBonusGame)
            {
                var gameModel = _mapper.Map<GameModel>(gameEntity);
                startedGames.Add(gameModel);
            }
            else
            {
                var bonusGames = await _gameRepository.GetAllBonusGames(userId);

                //Change list's last item
                bonusGames[bonusGames.Count - 1] = gameEntity;

                var bonusGamesModel = _mapper.Map<List<GameModel>>(bonusGames);

                startedGames.AddRange(bonusGamesModel);
            }

            return startedGames;
        }

        public async Task<bool> CashBack(GameModel model)
        {
            var gameEntity = _mapper.Map<Game>(model);

            var game = await _gameRepository.GetGameById(gameEntity.Id);

            if (game == null) return false;

            if (!CheckForCashback(game)) return false;

            game.IsCashback = true;

            //TODO: Logic for bonus game
            if(!game.IsBonusGame) game.IsEnded = true;

            //gameEntity Cashback to  User balance

            var result = await _gameRepository.UpdateGame(game);

            return result;
        }

        public async Task<GameModel> StartGame(GameModel model)
        {
            //TODO:Check User Balance
            //TODO: Should init User
            model.UserId = 8;  //Hard Code
            SetRange(model);

            var gameEntity = _mapper.Map<GameModel, Game>(model);

            if (model.IsBonusGame)
            {
                gameEntity.GameNumber = await GetGameNumber(model.UserId);
            }

            gameEntity.Coins = CreateCoins();
            
            if(model.IsBonusGame && gameEntity.GameNumber == GameConstants.BonusGameMaxNumber)
            {
                gameEntity.Coins = ShuffleForLastGame(gameEntity.Coins);
            }
            else
            {
                ShuffleCoins(gameEntity.Coins);
            }

            gameEntity = await _gameRepository.SaveGame(gameEntity);

            gameEntity.Coins = null;

            var gameModel = _mapper.Map<Game, GameModel>(gameEntity);

            return gameModel;
        }

        public async Task<bool> PrepareToStartNewGame(int userId)
        {
            var game = await _gameRepository.GetUserLastGame(userId);

            if (game == null) return true;

            game.IsEnded = true;

            await _gameRepository.UpdateGame(game);

            return true;
        }


        public async Task<CoinModel> GetCoinByPosition(CoinModel model)
        {
            var coinEntity = _mapper.Map<Coin>(model);

            coinEntity = await _gameRepository.GetCoinByPosition(coinEntity);

            if (coinEntity == null) return null;

            coinEntity.IsOpened = true;

            await _gameRepository.UpdateCoin(coinEntity);

            var gameEntity = await _gameRepository.GetGameById(model.GameId);

            //Check is game ended and update game

            await CheckEndGame(gameEntity);

            //await _gameRepository.UpdateGame(gameEntity);

            //Return only requsted coin 
            gameEntity.Coins = null;

            coinEntity.Game = gameEntity;

            var coinModel = _mapper.Map<Coin, CoinModel>(coinEntity);

            return coinModel;
        }

        public async Task<UserModel> GetUserById(int userId)
        {
            var user = await _gameRepository.GetUserById(userId);

            return _mapper.Map<User, UserModel>(user);
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

        private async Task CheckEndGame(Game game)
        {
            var openCoins = game.Coins.Where(x => x.IsOpened);
            var coinsSum = game.Coins.Where(x => x.IsOpened).Select(x => x.Value).Sum();

            if (openCoins.Count() == GameConstants.OpenedCoinsMaxCount)
            {
                if(!game.IsBonusGame)
                    game.IsEnded = true;
                else
                {
                    if(game.GameNumber == GameConstants.BonusGameMaxNumber)
                    {
                        await EndBonusGame(game);
                    }
                }

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
                if(coinsSum > game.EndRange)
                {
                    if(!game.IsBonusGame)
                    {
                        game.IsEnded = true;
                    }
                    else
                    {
                        if (game.GameNumber == GameConstants.BonusGameMaxNumber)
                        {
                            await EndBonusGame(game);
                        }
                    }
                    
                    game.IsWin = false;
                }
            }

            await _gameRepository.UpdateGame(game);
        }

        public async Task EndBonusGame(Game game)
        {
            var startedBonusGames = await _gameRepository.GetAllBonusGames(game.UserId);
            startedBonusGames.Select(c => { c.IsEnded = true; return c; }).ToList();

            await _gameRepository.UpdateGames(startedBonusGames);

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

        private List<Coin> ShuffleForLastGame(List<Coin> coins)
        {
            var shuffledList = new List<Coin>();
            var itemIndex = 0;
            for (int i = 0; i < GameConstants.MatrixHight; i++)
            {
                var row = new List<Coin>();
                
                for (int j = 0; j < GameConstants.MatrixWeight; j++)
                {
                    
                    row.Add(coins[0]);
                    coins.RemoveAt(0);
                }

                Random rnd = new Random();
                row = row.OrderBy(x => rnd.Next()).ToList();

                foreach (var item in row)
                {
                    item.Position = ++itemIndex;
                }

                shuffledList.AddRange(row);
            }
            return shuffledList;
        }

        private void SetRange(GameModel gameModel)
        {
            Random random = new Random();
            short randomNumber = (short)random.Next(GameConstants.MinNumber, GameConstants.MaxNumber);
            //string range = String.Format("{0} - {1}", randomNumber, randomNumber + GameConstants.Range);
            gameModel.StartRange = randomNumber;
            gameModel.EndRange = (short)(randomNumber + GameConstants.Range);
        }

        private async Task<short> GetGameNumber(int userId)
        {
            var lastGame = await _gameRepository.GetUserLastGame(userId);

            if (lastGame == null || !lastGame.IsBonusGame 
                || lastGame.GameNumber == GameConstants.BonusGameMaxNumber)
            {
                return (short)GameConstants.BonusGameMinNumber;
            }

            return ++lastGame.GameNumber;
        }

        #endregion
    }
}
