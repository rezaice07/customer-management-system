using ASPNETMVC5.Service.Services.Contacts;
using ASPNETMVC5.Service.Services.Users;
using Autofac;

namespace ASPNETMVC5.AWS.Infrastructure
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ContactService>().As<IContactService>().SingleInstance();
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();

            base.Load(builder);
        }
    }
}