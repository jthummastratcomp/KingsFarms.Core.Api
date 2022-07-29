using KingsFarms.Core.Api.Mappers;
using KingsFarms.Core.Api.Services.Interfaces;
using KingsFarms.Core.Api.ViewModels;
using Microsoft.Azure.Cosmos;

namespace KingsFarms.Core.Api.Services
{
    public class FieldOperationsService : IFieldOperationsService
    {
        private readonly string _cosmosDbContainer;
        private readonly string _cosmosDbDatabase;

        private readonly ICosmosDbService _cosmosDbService;

        private readonly Serilog.ILogger _logger;
        //private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];
        //private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];

        //private readonly CosmosClient _cosmosClient;
        //private readonly string containerId = "FieldOps";
        //private readonly string databaseId = "KingsDb";

        //private Database database;

        // The container we will create.
        //private Container container;

        public FieldOperationsService(string cosmosDbDatabase, string cosmosDbContainer, ICosmosDbService cosmosDbService, Serilog.ILogger logger)
        {
            _cosmosDbDatabase = cosmosDbDatabase;
            _cosmosDbContainer = cosmosDbContainer;
            _cosmosDbService = cosmosDbService;
            _logger = logger.ForContext<FieldOperationsService>();
            //_cosmosClient = new CosmosClient(EndpointUri, PrimaryKey,
            //    new CosmosClientOptions { ApplicationName = "CosmosDBKings" });
        }

        public async Task<List<BedHarvestFieldOpsViewModel>> GetBedsFieldOpsAsync()
        {
            var container = await _cosmosDbService.GetOrCreateCosmosDbContainerAsync(_cosmosDbDatabase, _cosmosDbContainer);

            var list = await GetWeeklyBedsFieldOpsAsync(container).ConfigureAwait(false);
            return list;
        }


        //public async Task<List<BedsMaintenanceCosmosDbModel>> GetBedsFieldOpsAsync()
        //{
        //    var container = _cosmosClient.GetContainer(databaseId, containerId);

        //    var list = await GetWeeklyMaintenanceAsync(container)
        //        .ConfigureAwait(false);
        //    return list;
        //}

        //public async Task AddBedsMaintenanceAsync()
        //{
        //    //await Program.Main(new []{"0"});

        //   var database = await CosmosDbDatabaseCreateAsync();
        //   var container = await CosmosDbContainerCreateAsync(database);

        //    for (var i = 1; i <= 20; i++)
        //    {
        //        await AddWeeklyBedsMaintenanceInfo(container, GetWeeklyBedMaintenance(i));
        //    }


        //    //return await Task.Run(() => GetWeeklyMaintenanceAsync(container));
        //    //return await GetWeeklyMaintenanceAsync(container);
        //}

        private async Task<List<BedHarvestFieldOpsViewModel>> GetWeeklyBedsFieldOpsAsync(Container container)
        {
            //ItemResponse<BedsMaintenanceCosmosDbModel> response = await container.ReadItemAsync<BedsMaintenanceCosmosDbModel>("1", new PartitionKey("Bed 1"));
            //return response.Resource;

            _logger.Information("GetWeeklyBedsFieldOpsAsync");

            var sql = "SELECT * FROM c WHERE c.partitionKey = 'Beds'";
            var query = new QueryDefinition(sql);

            var iterator = container.GetItemQueryIterator<BedHarvestFieldOpsCosmosDbModel>(query);

            var modelList = new List<BedHarvestFieldOpsCosmosDbModel>();

            while (iterator.HasMoreResults)
            {
                var results = await iterator.ReadNextAsync();
                foreach (var model in results)
                    modelList.Add(model);
                //Console.WriteLine("\tRead {0}\n", family);
            }

            return Mapper.MapBedHarvestFieldOpsCosmosDbModelToBedHarvestFieldOpsViewModel(modelList);
        }


        //private async Task<Database> CosmosDbDatabaseCreateAsync()
        //{
        //  var response = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
        //  return response.Database;
        //}

        //private async Task<Container> CosmosDbContainerCreateAsync(Database database)
        //{
        //    var response = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
        //    return response.Container;

        //}

        //private async Task AddWeeklyBedsMaintenanceInfo(Container container, BedsMaintenanceCosmosDbModel model)
        //{
        //    try
        //    {
        //        var response = await container.ReadItemAsync<BedsMaintenanceCosmosDbModel>(model.Id, new PartitionKey(model.PartitionKey));
        //    }
        //   catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        //    {
        //        var response = await container.CreateItemAsync(model, new PartitionKey(model.PartitionKey));
        //    }

        //}

        //private static BedsMaintenanceCosmosDbModel GetWeeklyBedMaintenance(int bedNumber)
        //{
        //    var model = new BedsMaintenanceCosmosDbModel()
        //    {
        //        Id = $"Bed{bedNumber}",
        //        PartitionKey = "Beds",
        //        PlantedDate = "10/01/2018",
        //        PlantsCount = 140,
        //        Section = SectionEnum.MidWest.ToString(),
        //        FieldOperations = new List<FieldOperationCosmosDbModel>()
        //        {
        //            new FieldOperationCosmosDbModel(){OperationDate = "11/08/2021", Type = FieldOperationEnum.Weeded.ToString()},
        //            new FieldOperationCosmosDbModel(){OperationDate = "11/08/2021", Type = FieldOperationEnum.Cleaned.ToString()},
        //            new FieldOperationCosmosDbModel(){OperationDate = "11/08/2021", Type = FieldOperationEnum.Aerated.ToString()},
        //            new FieldOperationCosmosDbModel(){OperationDate = "11/08/2021", Type = FieldOperationEnum.Fertilizer.ToString()},
        //            new FieldOperationCosmosDbModel(){OperationDate = "11/08/2021", Type = FieldOperationEnum.Weeded.ToString()},
        //        }.ToArray()
        //    };
        //    return model;
        //}

        //public List<BedsMaintenanceViewModel> GetBedsMaintenance()
        //{
        //    return new List<BedsMaintenanceViewModel>()
        //    {
        //        new BedsMaintenanceViewModel()
        //        {
        //            Id = 1, BedNumber = "1", PlantedDate = new DateTime(2018, 10, 1), PlantsCount = 196,
        //            Section = SectionEnum.MidWest,
        //            FieldOperations = new List<FieldOperationViewModel>()
        //            {
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Weeded, OperationDate = new DateTime(2021, 11, 5) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Cleaned, OperationDate = new DateTime(2021, 11, 5) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Aerated, OperationDate = new DateTime(2021, 11, 5) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Fertilizer, OperationDate = new DateTime(2021, 10, 16) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.GrubControl, OperationDate = new DateTime(2021, 4, 26) },
        //            }
        //        },
        //        new BedsMaintenanceViewModel()
        //        {
        //            Id = 2, BedNumber = "2", PlantedDate = new DateTime(2018, 10, 1), PlantsCount = 196,
        //            Section = SectionEnum.MidWest,
        //            FieldOperations = new List<FieldOperationViewModel>()
        //            {
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Weeded, OperationDate = new DateTime(2021, 11, 5) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Cleaned, OperationDate = new DateTime(2021, 11, 5) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Aerated, OperationDate = new DateTime(2021, 11, 5) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.Fertilizer, OperationDate = new DateTime(2021, 10, 16) },
        //                new FieldOperationViewModel()
        //                    { Type = FieldOperationEnum.GrubControl, OperationDate = new DateTime(2021, 4, 26) },
        //            }
        //        }
        //    };
        //}
    }
}