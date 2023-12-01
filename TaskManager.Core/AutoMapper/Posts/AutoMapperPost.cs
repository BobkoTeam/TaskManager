using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTO_s.Post;
using TaskManager.Core.Entities.Site;

namespace TaskManager.Core.AutoMapper.Posts
{
    public class AutoMapperPost:Profile
    {
        public AutoMapperPost()
        {
            CreateMap<PostDto, Post>().ReverseMap();
        }
    }
}
