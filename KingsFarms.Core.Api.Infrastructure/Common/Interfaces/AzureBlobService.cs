using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using KingsFarms.Core.Api.UI;

namespace KingsFarms.Core.Api.Application.Common.Interfaces;

public class AzureBlobService : IAzureBlobService
{
    private readonly AppSettings _settings;


    public AzureBlobService(AppSettings settings)
    {
        _settings = settings;
    }
    public BlockBlobClient GetHarvestBlob()
    {
        var client = new BlobServiceClient(_settings.AzureSettings?.StorageConnectionString);
        var container = client.GetBlobContainerClient(_settings.AzureSettings?.StorageContainerName);
        var blob = container.GetBlockBlobClient(_settings.OneDriveSettings?.HarvestFile);

        return blob;
    }
}