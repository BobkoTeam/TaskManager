using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTO_s.User;
using TaskManager.Core.Entities.User;

namespace TaskManager.Core.AutoMapper.User
{
    public class AutoMapperUser:Profile
    {
        public AutoMapperUser()
        {
            CreateMap<UsersDto, AppUser>().ReverseMap();
            CreateMap<EditUserDto, AppUser>().ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            CreateMap<RegisterDto, AppUser>().ForMember(dst => dst.UserName, act => act.MapFrom(src => src.Email));
            CreateMap<AppUser, RegisterDto>();
        }
    }
}
