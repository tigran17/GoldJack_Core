using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Entities
{
    public class GameDetails
    {

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
