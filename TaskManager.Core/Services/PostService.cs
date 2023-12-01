using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTO_s.Post;
using TaskManager.Core.Entities.Site;
using TaskManager.Core.Entities.Specifications;
using TaskManager.Core.Entities.User;
using TaskManager.Core.Interfaces;

namespace TaskManager.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepository<Post> _postRepo;
        private readonly UserManager<AppUser> _userManager;
        
        public PostService(IConfiguration configuration, IRepository<Post> postRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _postRepo = postRepo;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
       
        }
        public async Task Create(PostDto model)
        {
          

            await _postRepo.Insert(_mapper.Map<Post>(model));
            await _postRepo.Save();
        }

        public async Task Delete(int id)
        {
            var currentPost = await Get(id);

            if (currentPost == null) return; // exception

            await _postRepo.Delete(id);
            await _postRepo.Save();
        }

        public async Task<PostDto?> Get(int id)
        {
            if (id < 0) return null; // exception handling

            var post = await _postRepo.GetByID(id);

            if (post == null) return null; // exception handling

            return _mapper.Map<PostDto>(post);
        }

        public async Task<List<PostDto>> GetAll(string currentUserId)
        {
            if (!string.IsNullOrEmpty(currentUserId))
            {
                var result = await _postRepo.GetListBySpec(new Posts.ByUserId(currentUserId));

                return _mapper.Map<List<PostDto>>(result);
            }
            return new List<PostDto>();
        }

        

        public async Task<PostDto> GetById(int id)
        {
            if (id < 0) return null; // exception handling

            var post = await _postRepo.GetByID(id);

            if (post == null) return null; // exception handling

            return _mapper.Map<PostDto>(post);
        }

       

        public Task<List<PostDto>> Search(string searchString)
        {
            throw new NotImplementedException();
        }

        public async Task Update(PostDto model)
        {
            await _postRepo.Update(_mapper.Map<Post>(model));
            await _postRepo.Save();
        }
    }
}
