using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Serilog;

namespace HotTowel.Web.Services
{
    public class CosmosDbService : ICosmosDbService
    {
        private readonly ILogger _logger;
        private readonly CosmosClient _cosmosClient;
        public CosmosDbService(string cosmosDbUri, string cosmosDbKey, ILogger logger)
        {
            _logger = logger;
            _cosmosClient = new CosmosClient(cosmosDbUri, cosmosDbKey, new CosmosClientOptions { ApplicationName = "CosmosDBKings" });
        }

        public async Task<Database> GetOrCreateCosmosDbDatabaseAsync(string databaseId)
        {
            _logger.Information("GetOrCreateCosmosDbDatabaseAsync");
            var response = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            _logger.Information("GetOrCreateCosmosDbDatabaseAsync completed");
            _logger.Information("CreateDatabaseIfNotExistsAsync {@RequestCharge}", response.RequestCharge);
            return response.Database;
        }

        public async Task<Container> GetOrCreateCosmosDbContainerAsync(string databaseId, string containerId)
        {
            var database = await GetOrCreateCosmosDbDatabaseAsync(databaseId);

            _logger.Information("GetOrCreateCosmosDbContainerAsync");
            var response = await database.CreateContainerIfNotExistsAsync(containerId, "/partitionKey");
            _logger.Information("CreateContainerIfNotExistsAsync {@RequestCharge}", response.RequestCharge);
            return response.Container;

        }
    }
}