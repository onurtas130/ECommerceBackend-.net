using Application.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    public static class DI_Redis
    {
        public static void AddRedisServices(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379,password=1234";
            });
            services.AddScoped<IRedisCacheService, RedisCacheService>();
        }
    }
}
