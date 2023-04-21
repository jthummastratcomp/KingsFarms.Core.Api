namespace KingsFarms.Core.Api.UI;

public class AppSettings
{
    public AzureSettings? AzureSettings { get; set; }
    public OneDriveSettings? OneDriveSettings { get; set; }
}

public class AzureSettings
{
    public string? StorageConnectionString { get; set; }
    public string? StorageContainerName { get; set; }
}

public class OneDriveSettings
{
    public string? OrdersFile { get; set; }
    public string? HarvestFile { get; set; }
    public string? ManureFile { get; set; }
}