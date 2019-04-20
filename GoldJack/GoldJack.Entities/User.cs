using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Entities
{
    public class User
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //[Required]
        public string PersonalId { get; set; }

        //[Required]
        public string Name { get; set; }

        //[Required]
        public string Surename { get; set; }

        //[Required]
        public string Login { get; set; }

        //[Required]
        public string Password { get; set; }

        //[Required]
        public string Email { get; set; }

        //[Required]
        public double Balance { get; set; }
    }
}
