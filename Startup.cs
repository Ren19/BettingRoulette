using BettingRoulette.Common.GenericClass;
using BettingRoulette.Infrastructure.Interfaces;
using BettingRoulette.Infrastructure.Repository;
using BettingRoulette.Infrastructure.Services;
using EasyCaching.InMemory;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;

namespace BettingRoulette
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        SecretSettings secretsSettings;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            secretsSettings = new SecretSettings();
            configuration.GetSection("SecretsSettings").Bind(secretsSettings);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Services HTTP API",
                    Version = Environment.GetEnvironmentVariable("VERSION"),
                    Description = "The Services Service HTTP API",
                });
            });
            services.AddControllers();
            services.AddScoped<IRouletteRepository, RouletteRepository>();
            services.AddScoped<IRouletteService, RouletteService>();
            services.AddSingleton<IDateFormat>(x => new DateFormat(Configuration["TimeZone"]));
            services.AddTransient<ILogRegisterService>(r => new LogRegisterService(secretsSettings));
            services.AddEasyCaching(options =>
            {
                options.UseInMemory("default");
                options.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        ExpirationScanFrequency = 300,
                        SizeLimit = 100,
                        EnableReadDeepClone = true,
                        EnableWriteDeepClone = false,
                    };
                    config.MaxRdSecond = 120;
                    config.EnableLogging = false;
                    config.LockMs = 5000;
                    config.SleepMs = 300;
                }, "default1");
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "api/masivian/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/api/masivian/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "api/masivian";

                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
