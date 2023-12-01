using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.AutoMapper.Posts;
using TaskManager.Core.AutoMapper.User;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Services;

namespace TaskManager.Core
{
    public static class ServiceExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddTransient<UserServices>();

            services.AddScoped<IPostService, PostService>();

        }
        public static void AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperUser));
            services.AddAutoMapper(typeof(AutoMapperPost));

        }
    }
}
