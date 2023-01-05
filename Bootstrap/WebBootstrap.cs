using Autofac;
using KingsFarms.Core.Api.Data.Db;
using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Providers;
using KingsFarms.Core.Api.Data.Repositories;
using KingsFarms.Core.Api.Services;
using KingsFarms.Core.Api.Services.Interfaces;
using Serilog;
using ILogger = Serilog.ILogger;

namespace KingsFarms.Core.Api.Bootstrap;

public class WebBootstrap : Module
{
    private readonly KingsFarmsCoreApiSettings _hotTowelCoreApiSettings;

    public WebBootstrap(KingsFarmsCoreApiSettings hotTowelCoreApiSettings)
    {
        _hotTowelCoreApiSettings = hotTowelCoreApiSettings;
    }

    protected override void Load(ContainerBuilder builder)
    {
        var seqLogUrl = _hotTowelCoreApiSettings.seqLogUrl;

        var azStoreConnStr = _hotTowelCoreApiSettings.azStoreConnStr;
        var azStoreContName = _hotTowelCoreApiSettings.azStoreContName;

        var weeklyOrdersUsdaFile = _hotTowelCoreApiSettings.weeklyOrdersUsdaFile;
        var harvestFile = _hotTowelCoreApiSettings.harvestFile;
        var horseManureFile = _hotTowelCoreApiSettings.horseManureFile;
        var fieldOperationsFile = _hotTowelCoreApiSettings.fieldOperationsFile;

        var fedexUrl = _hotTowelCoreApiSettings.fedexUrl;
        var fedexClientId = _hotTowelCoreApiSettings.fedexClientId;
        var fedexClientSecret = _hotTowelCoreApiSettings.fedexClientSecret;

        builder.Register<ILogger>((c, p) => new LoggerConfiguration().Enrich.WithProperty("App", "HotTowellette").WriteTo.Seq(seqLogUrl).CreateLogger());

        builder.RegisterType<WeeklyOrdersUsdaService>().As<IWeeklyOrdersUsdaService>()
            .WithParameter("azStoreConnStr", azStoreConnStr)
            .WithParameter("azStoreContName", azStoreContName)
            .WithParameter("weeklyOrdersUsdaFile", weeklyOrdersUsdaFile);

        builder.RegisterType<HarvestService>().As<IHarvestService>()
            .WithParameter("azStoreConnStr", azStoreConnStr)
            .WithParameter("azStoreContName", azStoreContName)
            .WithParameter("harvestFile", harvestFile);

        //builder.RegisterType<FieldOperationService>().As<IFieldOperationService>()
        //    .WithParameter("azStoreConnStr", azStoreConnStr)
        //    .WithParameter("azStoreContName", azStoreContName)
        //    .WithParameter("fieldOperationsFile", fieldOperationsFile);

        builder.RegisterType<HorseManureService>().As<IHorseManureService>()
            .WithParameter("azStoreConnStr", azStoreConnStr)
            .WithParameter("azStoreContName", azStoreContName)
            .WithParameter("horseManureFile", horseManureFile);

        builder.RegisterType<BedService>().As<IBedService>()
            .WithParameter("azStoreConnStr", azStoreConnStr)
            .WithParameter("azStoreContName", azStoreContName)
            .WithParameter("harvestFile", harvestFile);
        
        builder.RegisterType<FedexTokenService>().As<IFedexTokenService>()
            .WithParameter("url", fedexUrl)
            .WithParameter("clientId", fedexClientId)
            .WithParameter("clientSecret", fedexClientSecret);

        builder.RegisterType<FedexLocationService>().As<IFedexLocationService>()
            .WithParameter("url", fedexUrl);

        builder.RegisterType<FedexShipmentService>().As<IFedexShipmentService>()
            .WithParameter("url", fedexUrl);

        builder.RegisterType<PrepareUsdaInvoiceService>().As<IPrepareUsdaInvoiceService>();
        builder.RegisterType<InvoiceNumberGeneratorService>().As<IInvoiceNumberGeneratorService>();
        
        //builder.RegisterType<UsdaNewMemoService>().As<IUsdaMemoService>();
        builder.RegisterType<ApplyInvoiceInfoService>().As<IApplyInvoiceInfoService>();
        builder.RegisterType<InvoiceInfoService>().As<IInvoiceInfoService>();

        builder.RegisterType<CustomerDataProvider>().As<ICustomerDataProvider>();

        builder.RegisterType<Repository<Customer>>().As<IRepository<Customer>>();

        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

        builder.RegisterType<KingsFarmsDbContext>().As<IDbContext>();
    }
}