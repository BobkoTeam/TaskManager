using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTO_s.Post;

namespace TaskManager.Core.Interfaces
{
    public interface IPostService
    {
        Task<List<PostDto>> GetAll(string id);
        Task<PostDto?> Get(int id);
        Task Create(PostDto model);
        Task Update(PostDto model);
        Task Delete(int id);
        Task<PostDto> GetById(int id);

        Task<List<PostDto>> Search(string searchString);
    }
}
