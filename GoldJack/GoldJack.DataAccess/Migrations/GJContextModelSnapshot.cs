﻿// <auto-generated />
using GoldJack.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GoldJack.DataAccess.Migrations
{
    [DbContext(typeof(GJContext))]
    partial class GJContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GoldJack.Entities.Coin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameId");

                    b.Property<bool>("IsOpened");

                    b.Property<int>("Position");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Coins");
                });

            modelBuilder.Entity("GoldJack.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Bet");

                    b.Property<short>("EndRange");

                    b.Property<short>("GameNumber");

                    b.Property<bool>("IsBonusGame");

                    b.Property<bool>("IsCashback");

                    b.Property<bool>("IsEnded");

                    b.Property<bool>("IsWin");

                    b.Property<short>("StartRange");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("GoldJack.Entities.GameDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameId");

                    b.Property<string>("Key");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("GamesDetails");
                });

            modelBuilder.Entity("GoldJack.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Balance");

                    b.Property<string>("Email");

                    b.Property<string>("Login");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<string>("PersonalId");

                    b.Property<string>("Surename");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GoldJack.Entities.Coin", b =>
                {
                    b.HasOne("GoldJack.Entities.Game", "Game")
                        .WithMany("Coins")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GoldJack.Entities.GameDetails", b =>
                {
                    b.HasOne("GoldJack.Entities.Game", "Game")
                        .WithMany("GameDetails")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
