using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Server.Options;
using Server.Service;
using Server.Models;
using Server.Services;
using Server.StatisticStorege;

namespace Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TimeOptions>(Configuration.GetSection("Time"));
            services.AddSingleton<ISeriesService, SeriesService>();
            services.AddTransient<IRoundService, RoundService>();
            services.AddSingleton<IAccountStorage, AccountStorage>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton(provider => new JsonWorker<List<Account>>("accounts.json"));
            services.AddDbContext<StatisticContext>(options
                => options.UseSqlite("Data Source=Statistics.db"));
            services.AddTransient<Stopwatch>();
            services.AddTransient<IStatisticService,StatisticService>();
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
            });

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

