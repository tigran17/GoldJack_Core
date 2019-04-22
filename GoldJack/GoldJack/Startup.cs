using GoldJack.DataAccess.Context;
using GoldJack.DataAccess.Repositories;
using GoldJack.Interfaces.DAL;
using GoldJack.Interfaces.Services;
using GoldJack.Middlewares;
using GoldJack.Services;
using GoldJack.Utilities.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace GoldJack
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            #region Dependency Injection

            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IGameRepository, GameRepository>();
            services.AddDbContext<GJContext>(options => 
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"])
            );

            #endregion

            services.AddAutoMapper();
            services.AddOptions();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               // app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseExceptionMiddleware();
            app.UseMvc();
        }
    }
}
