using Autofac;
using KingsFarms.Core.Api.Services;
using KingsFarms.Core.Api.Services.Interfaces;
using Serilog;
using FedexShipmentService = KingsFarms.Core.Api.Services.FedexShipmentService;

namespace KingsFarms.Core.Api.Bootstrap
{
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
            //var logFile = ConfigurationBinder..AppSettings["logFile"];

            var cosmosDbUri = _hotTowelCoreApiSettings.cosmosDbUri;
            var cosmosDbKey = _hotTowelCoreApiSettings.cosmosDbKey;
            var cosmosDbContainer_Beds = "Beds1";
            var cosmosDbDatabaseId = "KingsDb1";

            var azStoreConnStr = _hotTowelCoreApiSettings.azStoreConnStr;
            var azStoreContName = _hotTowelCoreApiSettings.azStoreContName;
            var weeklyOrdersFile = _hotTowelCoreApiSettings.weeklyOrdersFile;
            var harvestFile = _hotTowelCoreApiSettings.harvestFile;
            var horseManureFile = _hotTowelCoreApiSettings.horseManureFile;
            var fieldOperationsFile = _hotTowelCoreApiSettings.fieldOperationsFile;

            var fedexUrl = _hotTowelCoreApiSettings.fedexUrl;
            var fedexClientId = _hotTowelCoreApiSettings.fedexClientId;
            var fedexClientSecret = _hotTowelCoreApiSettings.fedexClientSecret;

            builder.Register<Serilog.ILogger>((c, p) => new LoggerConfiguration().Enrich.WithProperty("App", "HotTowellette").WriteTo.Seq(seqLogUrl).CreateLogger());

            builder.RegisterType<WeeklyOrdersService>().As<IWeeklyOrdersService>()
                .WithParameter("azStoreConnStr", azStoreConnStr)
                .WithParameter("azStoreContName", azStoreContName)
                .WithParameter("weeklyOrdersFile", weeklyOrdersFile);

            builder.RegisterType<HarvestService>().As<IHarvestService>()
                .WithParameter("azStoreConnStr", azStoreConnStr)
                .WithParameter("azStoreContName", azStoreContName)
                .WithParameter("harvestFile", harvestFile);

            builder.RegisterType<FieldOperationService>().As<IFieldOperationService>()
                .WithParameter("azStoreConnStr", azStoreConnStr)
                .WithParameter("azStoreContName", azStoreContName)
                .WithParameter("fieldOperationsFile", fieldOperationsFile);

            builder.RegisterType<HorseManureService>().As<IHorseManureService>()
                .WithParameter("azStoreConnStr", azStoreConnStr)
                .WithParameter("azStoreContName", azStoreContName)
                .WithParameter("horseManureFile", horseManureFile);

            builder.RegisterType<BedService>().As<IBedService>()
                .WithParameter("azStoreConnStr", azStoreConnStr)
                .WithParameter("azStoreContName", azStoreContName)
                .WithParameter("harvestFile", harvestFile);

            builder.RegisterType<CosmosDbService>().As<ICosmosDbService>()
                .WithParameter("cosmosDbUri", cosmosDbUri)
                .WithParameter("cosmosDbKey", cosmosDbKey);

            builder.RegisterType<BedHarvestFieldOpsService>().As<IBedHarvestFieldOpsService>()
                .WithParameter("cosmosDbDatabase", cosmosDbDatabaseId)
                .WithParameter("cosmosDbContainer", cosmosDbContainer_Beds);

            builder.RegisterType<FieldOperationsService>().As<IFieldOperationsService>()
                .WithParameter("cosmosDbDatabase", cosmosDbDatabaseId)
                .WithParameter("cosmosDbContainer", cosmosDbContainer_Beds);

            builder.RegisterType<FedexTokenService>().As<IFedexTokenService>()
                .WithParameter("url", fedexUrl)
                .WithParameter("clientId", fedexClientId)
                .WithParameter("clientSecret", fedexClientSecret);

            builder.RegisterType<FedexLocationService>().As<IFedexLocationService>()
                .WithParameter("url", fedexUrl);

            builder.RegisterType<FedexShipmentService>().As<IFedexShipmentService>()
                .WithParameter("url", fedexUrl);

            builder.RegisterType<PrepareInvoiceService>().As<IPrepareInvoiceService>();
            builder.RegisterType<InvoiceNumberGeneratorService>().As<IInvoiceNumberGeneratorService>();
            builder.RegisterType<UsdaQueueService>().As<IUsdaQueueService>();
            builder.RegisterType<UsdaMemoService>().As<IUsdaMemoService>();
            builder.RegisterType<ApplyInvoiceInfoService>().As<IApplyInvoiceInfoService>();
            builder.RegisterType<InvoiceInfoService>().As<IInvoiceInfoService>();

        }
    }
}