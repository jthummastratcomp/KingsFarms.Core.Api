using System.Configuration;
using Autofac;

using HotTowel.Web.Helpers;
using HotTowel.Web.Services;
using HotTowel.Web.Services.Interfaces;
using Serilog;

//using Ninject.Modules;
//using Ninject.Web.Common;
//using Serilog;

namespace HotTowel.Web.Bootstrap
{
    public class WebBootstrap : Module
    {
        private readonly SomeSettings _someSettings;

        public WebBootstrap(SomeSettings someSettings)
        {
            _someSettings = someSettings;

        }

        protected override void Load(ContainerBuilder builder)
        {

            var seqLogUrl = _someSettings.seqLogUrl;
            //var logFile = ConfigurationBinder..AppSettings["logFile"];

            var cosmosDbUri = _someSettings.cosmosDbUri;
            var cosmosDbKey = _someSettings.cosmosDbKey;
            var cosmosDbContainer_Beds = "Beds1";
            var cosmosDbDatabaseId = "KingsDb1";

            var azStoreConnStr = _someSettings.azStoreConnStr;
            var azStoreContName = _someSettings.azStoreContName;
            var weeklyOrdersFile = _someSettings.weeklyOrdersFile;
            var harvestFile = _someSettings.harvestFile;

            builder.Register<Serilog.ILogger>((c, p) => new LoggerConfiguration().Enrich.WithProperty("App", "HotTowellette").WriteTo.Seq(seqLogUrl).CreateLogger());

            builder.RegisterType<WeeklyOrdersService>().As<IWeeklyOrdersService>()
                .WithParameter("azStoreConnStr", azStoreConnStr)
                .WithParameter("azStoreContName", azStoreContName)
                .WithParameter("weeklyOrdersFile", weeklyOrdersFile);

            builder.RegisterType<HarvestService>().As<IHarvestService>()
                .WithParameter("azStoreConnStr", azStoreConnStr)
                .WithParameter("azStoreContName", azStoreContName)
                .WithParameter("harvestFile", harvestFile);

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