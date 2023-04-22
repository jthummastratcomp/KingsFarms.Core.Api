using Autofac;
using KingsFarms.Core.Api.Application;
using KingsFarms.Core.Api.Application.Beds;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;

namespace KingsFarms.Core.Api.UI;

public class ConfigureUIServices : Module
{
    private readonly AppSettings? _appSettings;


    public ConfigureUIServices(AppSettings? appSettings)
    {
        _appSettings = appSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterMediatR(MediatRConfigurationBuilder
            .Create(typeof(GetBedsQuery).Assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithRegistrationScope(RegistrationScope.Scoped)
            .Build());

        builder.RegisterModule(new ConfigureApplicationServices(_appSettings));

        base.Load(builder);
    }
}