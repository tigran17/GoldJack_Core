using AutoMapper;
using GoldJack.Entities;
using GoldJack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldJack.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile ()
        {
            CreateMap<Game, GameModel>();
            CreateMap<GameModel, Game>();

            CreateMap<Coin, CoinModel>();
            CreateMap<CoinModel, Coin>();

            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
        }
            

           
    }
}
