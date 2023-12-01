using Ardalis.Specification;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Site;

namespace TaskManager.Core.Entities.Specifications
{
    public class Posts:Specification<Post>
    {
        

        
        public class ByUserId:Specification<Post>
        {
            private readonly string _userId;
            public ByUserId(string userId)
            {

                Query
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.Id);

            }
        }

        
    }
}
