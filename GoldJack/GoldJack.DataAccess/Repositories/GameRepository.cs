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
        public async Task<Game> GetGame(Game game)
        {
            var gameEntity = new Game();

            using (_context)
            {
                gameEntity = _context.Games.Where(x => x.UserId == game.UserId).LastOrDefault();

                if(gameEntity == null || gameEntity.IsEnded)
                {
                    return game;
                }
                else
                {
                    //TODO: Should be Implemented
                    return game;
                }
            }
        }

        public async Task<Game> SaveGame(Game game)
        {
            Game gameEntity;

            using (_context)
            {
                _context.Games.Add(game);
                _context.SaveChanges();

                gameEntity = _context.Games.Where(x => x.UserId == game.UserId)
                    .OrderByDescending(x => x.Id).FirstOrDefault();

                SaveGameCoins(gameEntity);
            }

            return gameEntity;
        }

        public async Task<Coin> GetCoinByPosition(Coin coin)
        {
            using (_context)
            {
                coin =  _context.Coins.Where(x => x.GameId == coin.GameId && x.Position == coin.Position)
                    .FirstOrDefault();

                if (coin == null) return null;

                coin.IsOpened = true;

                _context.SaveChanges();
            }

            return coin;
        }


        //private functions
        #region 

        private void SaveGameCoins(Game game)
        {
            List<Coin> coins = new List<Coin>();

            foreach(var coin in game.Coins)
            {
                var coinEntity = new Coin
                {
                    GameId = game.Id,
                    Position = coin.Position,
                    Value = coin.Value
                };

                coins.Add(coinEntity);
            }


            _context.Coins.AddRange(coins);
            _context.SaveChanges();
        }

        #endregion

    }
}
