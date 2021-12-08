using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extentions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>
               (options =>
               {
                   options.UseSqlite(config.GetConnectionString("DefaultConnection"));
               });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);   //goes and finds the profiles


            return services;
        }

    }
}


//Anti gia auta -> UoW
// services.AddScoped<IUserRepo, UserRepo>();
// services.AddScoped<ILikesRepo, LikesRepo>();
// services.AddScoped<IMessageRepo, MessageRepo>();