using Autofac;
using BusinessLogic.Interfaces;
using BusinessLogic.Managers;

namespace BusinessLogic
{
    public class BusinessLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GuildTeamManager>().As<IGuildTeamManager>().PropertiesAutowired();
        }   
    }
}
