using GoldJack.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldJack.DataAccess.Context
{
    public class GJContext : DbContext
    {
        public GJContext (DbContextOptions<GJContext> context) : base(context)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB;Database=GoldJack;Trusted_Connection=True;");

        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        //entities

        public DbSet<Game> Games { get; set; }
        public DbSet<GameDetails> GamesDetails { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<User> Users { get; set; }

        //public DbSet<GameCoins> GamesCoins { get; set; } 

    }
}
