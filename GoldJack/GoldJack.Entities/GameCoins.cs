using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Entities
{
    public class GameCoins
    {
        public int Id { get; set; }
        public int CoinId { get; set; }
        public Coin Coin { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
