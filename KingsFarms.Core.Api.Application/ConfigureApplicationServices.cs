using Autofac;
using Autofac.Core;
using KingsFarms.Core.Api.Infrastructure;
using System.Reflection;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Module = Autofac.Module;
using KingsFarms.Core.Api.UI;

namespace KingsFarms.Core.Api.Application;

public class ConfigureApplicationServices : Module
{
    private readonly AppSettings? _appSettings;

    public ConfigureApplicationServices(AppSettings? appSettings)
    {
        _appSettings = appSettings;
    }
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsClosedTypesOf(typeof(IRequestHandler<>));
        //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsClosedTypesOf(typeof(IRequestHandler<,>));
        builder.RegisterAutoMapper(Assembly.GetExecutingAssembly());

        builder.RegisterModule(new ConfigureInfrastructureServices(_appSettings));

        base.Load(builder);
    }
}