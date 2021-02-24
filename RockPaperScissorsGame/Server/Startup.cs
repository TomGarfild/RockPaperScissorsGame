using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Server.Model;
using Server.Options;
using Server.Service;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Server.Models;
using Server.Services;

namespace Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
      
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TimeOptions>(Configuration.GetSection("Time"));
            services.AddSingleton<ISeriesService, SeriesService>();
            services.AddTransient<IRoundService, RoundService>();
            services.AddSingleton<IAccountStorage, AccountStorage>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton(provider => new JsonWorker<List<Account>>("accounts.json"));

            services.AddMemoryCache();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DI Demo App API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DI Demo App API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

