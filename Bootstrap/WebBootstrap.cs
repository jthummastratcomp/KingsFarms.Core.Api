using Autofac;
using HotTowel.Core.Api.Services;
using HotTowel.Core.Api.Services.Interfaces;
using Serilog;

namespace HotTowel.Core.Api.Bootstrap
{
    public class WebBootstrap : Module
    {
        private readonly HotTowelCoreApiSettings _hotTowelCoreApiSettings;

        public WebBootstrap(HotTowelCoreApiSettings hotTowelCoreApiSettings)
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


            //Bind<ILogger>().ToMethod(x => new LoggerConfiguration()
            //    //.WriteTo.File(logFile)
            //    .Enrich.WithProperty("App", "HotTowellette")
            //    .WriteTo.Seq(seqLogUrl)
            //    //.WriteTo.AzureBlobStorage(azStoreConnStr,storageContainerName: "hottowellettewevlogs",storageFileName:logFile)
            //    .CreateLogger());

            //Bind<IIntuitDataService>().To<IntuitDataService>().InRequestScope(); //.InSingletonScope();
            //Bind<IAdminService>().To<AdminService>();
            //Bind<ICustomerService>().To<CustomerService>();
            //Bind<ICustomerLoadService>().To<CustomerLoadService>();
            //Bind<IInvoiceService>().To<InvoiceService>();
            //Bind<IInvoiceLoadService>().To<InvoiceLoadService>();
            //Bind<IInvoiceLoadLoopService>().To<InvoiceLoadLoopService>();
            //Bind<IWeeklyOrdersService>().To<WeeklyOrdersService>()
            //    .WithConstructorArgument("azStoreConnStr", azStoreConnStr)
            //    .WithConstructorArgument("azStoreContName", azStoreContName)
            //    .WithConstructorArgument("weeklyOrdersFile", weeklyOrdersFile);
            //Bind<IGenerateWeeklyOrdersService>().To<GenerateWeeklyOrdersService>();

            //Bind<IHarvestService>().To<HarvestService>()
            //    .WithConstructorArgument("azStoreConnStr", azStoreConnStr)
            //    .WithConstructorArgument("azStoreContName", azStoreContName)
            //    .WithConstructorArgument("harvestFile", harvestFile);

            //Bind<ICosmosDbService>().To<CosmosDbService>()
            //    .WithConstructorArgument("cosmosDbUri", cosmosDbUri)
            //    .WithConstructorArgument("cosmosDbKey", cosmosDbKey);
            //Bind<IBedHarvestFieldOpsService>().To<BedHarvestFieldOpsService>()
            //    .WithConstructorArgument("cosmosDbDatabase", cosmosDbDatabaseId)
            //    .WithConstructorArgument("cosmosDbContainer", cosmosDbContainer_Beds);
            //Bind<IFieldOperationsService>().To<FieldOperationsService>()
            //    .WithConstructorArgument("cosmosDbDatabase", cosmosDbDatabaseId)
            //    .WithConstructorArgument("cosmosDbContainer", cosmosDbContainer_Beds);

            //Bind<ICacheProvider>().To<MemoryCacheProvider>().WithConstructorArgument("cacheTimeoutMinutes", 20); //.InSingletonScope()

            //Bind<ICacheInterceptor>().To<CacheInterceptor>();
            ////Bind<ICacheExpireInterceptor>().To<CacheExpireInterceptor>();
        }
    }
}