using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using BusinessLogic;
using DataAccess;
using DataAccess.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhoGivesACluck.Middleware;
using WhoGivesACluck.Services;

namespace WhoGivesACluck
{
    public class Startup
    {
        private IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment hostEnvironment,
            CluckContext dbContext)
        {
            applicationBuilder.UseRouting();
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            applicationBuilder.UseHttpLogging();
            applicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<BusinessLogicModule>();

            builder.RegisterType<BotService>().AsSelf().PropertiesAutowired();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AssignableTo<ControllerBase>()
                .InstancePerLifetimeScope()
                .PropertiesAutowired();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_configuration);
            services.AddControllers()
                .AddControllersAsServices();
            services.AddAutoMapper();
            services.AddHostedService<BotService>();
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<CluckContext>(opt =>
                {
                    opt.UseSqlite(_configuration.GetConnectionString("Default"));
                });
            services.AddMvc();
            
        }
    }
}

