using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.User;
using TaskManager.Core.Interfaces;

namespace TaskManager.Core.Entities.Site
{
    public class Post:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Date { get; set; } = string.Empty;
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
