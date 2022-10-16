using Microsoft.Azure.Cosmos;

namespace KingsFarms.Core.Api.Services.Interfaces;

public interface ICosmosDbService
{
    Task<Database> GetOrCreateCosmosDbDatabaseAsync(string databaseId);
    Task<Container> GetOrCreateCosmosDbContainerAsync(string databaseId, string containerId);
}