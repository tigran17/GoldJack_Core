using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Models
{
    public class GameModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public double Bet { get; set; }

        public string Range { get; set; }

        public short StartRange { get; set; }
        public short EndRange { get; set; }

        public bool IsWin { get; set; }

        public bool IsEnded { get; set; }

        public bool IsCashback { get; set; }

        public bool IsBonusGame { get; set; }

        public Int16 GameNumber { get; set; }

        public double UserBalance { get; set; }
    }
}
