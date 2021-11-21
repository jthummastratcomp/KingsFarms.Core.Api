using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HotTowel.Web.Helpers;
using HotTowel.Web.Services.Interfaces;
using HotTowel.Web.ViewModels;
using Microsoft.Azure.Cosmos;
using Serilog;

namespace HotTowel.Web.Services
{
    public class BedHarvestFieldOpsService : IBedHarvestFieldOpsService
    {
        private readonly string _cosmosDbContainer;
        private readonly Serilog.ILogger _logger;
        private readonly string _cosmosDbDatabase;
        private readonly ICosmosDbService _cosmosDbService;
        private readonly IHarvestService _harvestService;


        public BedHarvestFieldOpsService(Serilog.ILogger logger, string cosmosDbDatabase, string cosmosDbContainer, ICosmosDbService cosmosDbService, IHarvestService harvestService)
        {
            _logger = logger.ForContext<BedHarvestFieldOpsService>();
            _cosmosDbDatabase = cosmosDbDatabase;
            _cosmosDbContainer = cosmosDbContainer;
            _cosmosDbService = cosmosDbService;
            _harvestService = harvestService;
        }

        public async Task<List<BedHarvestFieldOpsViewModel>> GetOrAddBedInfoToCosmosDbAsync()
        {
            var bedList = _harvestService.GetBedInfo();

            var container = await _cosmosDbService.GetOrCreateCosmosDbContainerAsync(_cosmosDbDatabase, _cosmosDbContainer);

            var list = await AddOrUpdateBedInfoAsync(container, bedList).ConfigureAwait(false);
            return list;
        }

        private async Task<List<BedHarvestFieldOpsViewModel>> AddOrUpdateBedInfoAsync(Container container, List<BedHarvestFieldOpsViewModel> bedList)
        {
            if (!Utils.HasRows(bedList)) return new List<BedHarvestFieldOpsViewModel>();

            var modelList = new List<BedHarvestFieldOpsCosmosDbModel>();
            foreach (var viewModel in bedList)
            {
                if (string.IsNullOrEmpty(viewModel.PartitionKey)) viewModel.PartitionKey = "Beds";
                modelList.Add(await AddOrUpdateBedAsync(container, Mapper.MapBedHarvestFieldOpsViewModelToBedHarvestFieldOpsCosmosDbModel(viewModel)));
            }

            return Mapper.MapBedHarvestFieldOpsCosmosDbModelToBedHarvestFieldOpsViewModel(modelList);
        }

        private async Task<BedHarvestFieldOpsCosmosDbModel> AddOrUpdateBedAsync(Container container, BedHarvestFieldOpsCosmosDbModel model)
        {
            try
            {
                var response = await container.ReadItemAsync<BedHarvestFieldOpsCosmosDbModel>(model.Id, new PartitionKey(model.PartitionKey));
                var itemBody = response.Resource;
                _logger.Information("ReadItemAsync {@Id} took {@RequestCharge} RU", model.Id, response.RequestCharge);

                if (itemBody.Display != model.Display)
                {
                    //response = await container.ReplaceItemAsync(itemBody, itemBody.Id, new PartitionKey(itemBody.PartitionKey));
                    //return response.Resource;

                    response = await container.DeleteItemAsync<BedHarvestFieldOpsCosmosDbModel>(model.Id, new PartitionKey(model.PartitionKey));
                    _logger.Information("DeleteItemAsync {@Id} took {@RequestCharge} RU", model.Id, response.RequestCharge);
                    return await CreateItem(container, model);
                }

                _logger.Information("Model is same for {@Id}", model.Id);
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return await CreateItem(container, model);
            }
        }

        private  async Task<BedHarvestFieldOpsCosmosDbModel> CreateItem(Container container, BedHarvestFieldOpsCosmosDbModel model)
        {
            var response = await container.CreateItemAsync(model, new PartitionKey(model.PartitionKey));
            _logger.Information("CreateItemAsync {@Id} took {@RequestCharge} RU", model.Id, response.RequestCharge);
            return response.Resource;
        }


        //private async Task<List<BedsMaintenanceCosmosDbModel>> GetWeeklyMaintenanceAsync(Container container)
        //{
        //    //ItemResponse<BedsMaintenanceCosmosDbModel> response = await container.ReadItemAsync<BedsMaintenanceCosmosDbModel>("1", new PartitionKey("Bed 1"));
        //    //return response.Resource;

        //    var sqlQueryText = "SELECT * FROM c WHERE c.partitionKey = 'Beds'";
        //    QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

        //    FeedIterator<BedsMaintenanceCosmosDbModel> queryResultSetIterator = container.GetItemQueryIterator<BedsMaintenanceCosmosDbModel>(queryDefinition);

        //    List<BedsMaintenanceCosmosDbModel> families = new List<BedsMaintenanceCosmosDbModel>();

        //    while (queryResultSetIterator.HasMoreResults)
        //    {
        //        FeedResponse<BedsMaintenanceCosmosDbModel> currentResultSet = await queryResultSetIterator.ReadNextAsync();
        //        foreach (BedsMaintenanceCosmosDbModel family in currentResultSet)
        //        {
        //            families.Add(family);
        //            //Console.WriteLine("\tRead {0}\n", family);
        //        }
        //    }

        //    return families;
        //}

        //public IHarvestService AddHarvestInfoToCosmosDb()
        //{

        //}

        //public IHarvestService AddFieldOpsInfoToCosmosDb()
        //{

        //}
    }
}