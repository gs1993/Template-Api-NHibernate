﻿using Api.Utils;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api
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
            services.AddMvc();

            services.AddSingleton(new SessionFactory(Configuration["ConnectionString"]));
            services.AddScoped<UnitOfWork>();
            services.AddTransient<MovieRepository>();
            services.AddTransient<CustomerRepository>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors(builder => builder
                .WithOrigins(Consts.ClientAppUrl)
                .AllowAnyHeader()
                .AllowAnyMethod());

            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
