using Azure.Storage.Blobs.Specialized;

namespace KingsFarms.Core.Api.Application.Common.Interfaces;

public interface IAzureBlobService
{
    BlockBlobClient GetHarvestBlob();
}