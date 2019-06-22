using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string PersonalId { get; set; }

        public string Name { get; set; }

        public string Surename { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public double Balance { get; set; }
    }
}
