using GoldJack.DataAccess.Context;
using GoldJack.Entities;
using GoldJack.Interfaces.DAL;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> UpdateGames(List<Game> gameEntities)
        {
            _context.UpdateRange(gameEntities);

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
           var gameEntity = await _context.Games.Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            return gameEntity;
        }

        public async Task<List<Coin>> GetGameOpenedCoins(int gameId)
        {
            var coins = await _context.Coins.Where(x => x.GameId == gameId && x.IsOpened)
                .ToListAsync();

            return coins;
        }

        public async Task<Game> SaveGame(Game game)
        {           
            //TODO: Investigation
            _context.Games.Add(game);
            await _context.SaveAsync();

            return game;
        }

        public async Task<Coin> GetCoinByPosition(Coin coin)
        {
            coin =  await _context.Coins
                .Where(x => x.GameId == coin.GameId && x.Position == coin.Position)
                .FirstOrDefaultAsync();

            if (coin == null) return null;                

            await _context.SaveAsync();
            return coin;
        }

        public async Task<List<Game>> GetAllBonusGames(int userId)
        {
            var allBonusGames = await _context.Games
                .Where(x => x.UserId == userId && x.IsBonusGame && !x.IsEnded)
                //.Select(i => new Game {
                //    GameNumber = i.GameNumber,
                //    IsBonusGame = i.IsBonusGame,
                //    IsCashback = i.IsCashback,
                //    IsWin = i.IsWin
                //})
                .OrderBy(g => g.Id)
                .ToListAsync();

            return allBonusGames;
        }

        public async Task<Game> GetGameById(int gameId)
        {
            var gameEntity = await  _context.Games.Where(x => x.Id == gameId)
                .Include(c => c.Coins)
                .FirstOrDefaultAsync();

            return gameEntity;
        }

        public async Task<User> GetUserById(int userId)
        {
            var userEntity = await _context.Users.Where(x => x.Id == userId)
                .Select(x => new User { Id = x.Id, Name = x.Name, Surename = x.Surename, Balance = x.Balance })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return userEntity;
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
