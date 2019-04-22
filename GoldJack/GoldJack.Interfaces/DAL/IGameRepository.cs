using GoldJack.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoldJack.Interfaces.DAL
{
    public interface IGameRepository
    {
        Task<Game> GetGame(Game gameEntity);
        Task<Game> SaveGame(Game gameEntity);
        Task<Coin> GetCoinByPosition(Coin coinEntity);
    }
}
