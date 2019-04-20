using GoldJack.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoldJack.Interfaces.Services
{
    public interface IGameService
    {
        Task<GameModel> GetGame(GameModel mode);
        Task<GameModel> StartGame(GameModel model);
    }
}
