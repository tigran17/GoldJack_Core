using GoldJack.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoldJack.Interfaces.Services
{
    public interface IGameService
    {
        Task<GameModel> GetGame();
        Task<GameModel> StartGame(GameModel model);
        Task<CoinModel> GetCoinByPosition(CoinModel model);
        Task<bool> CashBack(GameModel model);
    }
}
