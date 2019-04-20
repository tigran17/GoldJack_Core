using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Entities
{
    public class Coin
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int GameId { get; set; }

        //[ForeignKey("GameId")]
        public Game Game { get; set; }

        public int Position { get; set; }

        public int Value { get; set; }

        public bool IsOpened { get; set; }
    }
}
