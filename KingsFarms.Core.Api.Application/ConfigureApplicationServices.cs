using System.Reflection;
using Autofac;
using KingsFarms.Core.Api.Infrastructure;
using MediatR;
using Module = Autofac.Module;

namespace KingsFarms.Core.Api.Application;

public class ConfigureApplicationServices : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsClosedTypesOf(typeof(IRequestHandler<>));
        //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsClosedTypesOf(typeof(IRequestHandler<,>));

        builder.RegisterModule(new ConfigureInfrastructureServices());

        base.Load(builder);
    }
}