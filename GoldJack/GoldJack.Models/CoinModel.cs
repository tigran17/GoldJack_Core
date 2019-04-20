using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Models
{
    public class CoinModel
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public int Position { get; set; }

        public int Value { get; set; }

        public bool IsOpened { get; set; }
    }
}
