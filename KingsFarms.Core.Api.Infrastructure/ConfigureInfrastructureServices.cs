using System.Diagnostics;
using Autofac;
using KingsFarms.Core.Api.Application.Common.Interfaces;
using KingsFarms.Core.Api.UI;

namespace KingsFarms.Core.Api.Infrastructure;

public class ConfigureInfrastructureServices : Module
{
    private readonly AppSettings? _appSettings;

    public ConfigureInfrastructureServices(AppSettings? appSettings)
    {
        _appSettings = appSettings;
    }
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ApplicationContext>().As<IApplicationContext>();

        
        builder.RegisterType<AzureBlobService>().As<IAzureBlobService>().WithParameter("settings", _appSettings);
        builder.RegisterType<HarvestContext>().As<IHarvestContext>();

        base.Load(builder);
    }
}