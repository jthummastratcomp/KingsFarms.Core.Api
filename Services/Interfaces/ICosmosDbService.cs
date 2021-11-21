using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace HotTowel.Web.Services
{
    public interface ICosmosDbService
    {
        Task<Database> GetOrCreateCosmosDbDatabaseAsync(string databaseId);
        Task<Container> GetOrCreateCosmosDbContainerAsync(string databaseId, string containerId);
    }
}