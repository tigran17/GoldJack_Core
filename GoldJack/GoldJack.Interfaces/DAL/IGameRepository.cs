using GoldJack.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoldJack.Interfaces.DAL
{
    public interface IGameRepository
    {
        Task<User> GetUserById(int userId);
        Task<Game> SaveGame(Game gameEntity);
        Task<Coin> GetCoinByPosition(Coin coinEntity);
        Task<Game> GetGameById(int gameId);
        Task<Game> GetUserLastGame(int userId);
        Task<bool> UpdateGame(Game gameEntity);
        Task<bool> UpdateGames(List<Game> gameEntity);
        Task UpdateCoin(Coin coinEntity);
        Task<List<Coin>> GetGameOpenedCoins(int gameId);
        Task<List<Game>> GetAllBonusGames(int userId);
    }
}
