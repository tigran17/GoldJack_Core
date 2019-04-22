using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Entities
{
    public class Game
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        //[ForeignKey("UserId")]
        //public User User { get; set; }

        public double Bet { get; set; }

        public Int16 StartRange { get; set; }

        public Int16 EndRange { get; set; }

        public bool IsWin { get; set; }

        public bool IsCashback { get; set; }

        public bool IsBonusGame { get; set; }

        public Int16 GameNumber { get; set; }

        public bool IsEnded { get; set; }

        public List<Coin> Coins { get; set; }

        public List<GameDetails> GameDetails { get; set; }
    }
}
