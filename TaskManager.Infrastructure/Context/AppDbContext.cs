using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Entities.Site;
using TaskManager.Core.Entities.User;

namespace TaskManager.Infrastructure.Context
{
    internal class AppDbContext:IdentityDbContext
    {
        public AppDbContext() : base()
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
