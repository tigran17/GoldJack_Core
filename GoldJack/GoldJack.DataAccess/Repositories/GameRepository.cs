using GoldJack.DataAccess.Context;
using GoldJack.Entities;
using GoldJack.Interfaces.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoldJack.DataAccess.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GJContext _context;

        public GameRepository(GJContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateGame(Game gameEntity)
        {
            _context.Update<Game>(gameEntity);

            await _context.SaveAsync();

            return true;
        }

        public async Task UpdateCoin(Coin coinEntity)
        {
            _context.Update<Coin>(coinEntity);

            await _context.SaveAsync();
        }
        public async Task<Game> GetUserLastGame(int userId)
        { 
           var gameEntity = _context.Games.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id).FirstOrDefault();

            return gameEntity;
        }

        public async Task<List<Coin>> GetGameOpenedCoins(int gameId)
        {
            var coins = _context.Coins.Where(x => x.GameId == gameId && x.IsOpened).ToList();

            return coins;
        }

        public async Task<Game> SaveGame(Game game)
        {           
            //TODO: Investigation
            _context.Games.Add(game);
            await _context.SaveAsync();

            var gameEntity = _context.Games.Where(x => x.UserId == game.UserId)
                    .OrderByDescending(x => x.Id).FirstOrDefault();

           // await SaveGameCoins(gameEntity);

            return gameEntity;
        }

        public async Task<Coin> GetCoinByPosition(Coin coin)
        {
            coin =  _context.Coins
                .Where(x => x.GameId == coin.GameId && x.Position == coin.Position)
                .FirstOrDefault();

            if (coin == null) return null;                

            await _context.SaveAsync();
            return coin;
        }

        public async Task<Game> GetGameById(int gameId)
        {
            var gameEntity = _context.Games.Where(x => x.Id == gameId).FirstOrDefault();

            return gameEntity;
        }

        //private functions
        #region 

        //private async Task SaveGameCoins(Game game)
        //{
        //    List<Coin> coins = new List<Coin>();

        //    foreach(var coin in game.Coins)
        //    {
        //        var coinEntity = new Coin
        //        {
        //            GameId = game.Id,
        //            Position = coin.Position,
        //            Value = coin.Value
        //        };

        //        coins.Add(coinEntity);
        //    }


        //    _context.Coins.AddRange(coins);
        //    await _context.SaveAsync();
        //}

        #endregion

    }
}
