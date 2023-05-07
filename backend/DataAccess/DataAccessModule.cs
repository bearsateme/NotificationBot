using Autofac;
using DataAccess.Interfaces;
using DataAccess.Repositories;

namespace DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GuildTeamRepository>().As<IGuildTeamRepository>().PropertiesAutowired();
        }   
    }
}
