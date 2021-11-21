public class HotTowelCoreApiSettings
{
    public string seqLogUrl { get; set; }
    public string logFile { get; set; }

    public string cosmosDbUri { get; set; }
    public string cosmosDbKey { get; set; }
    public string cosmosDbContainer_Beds { get; set; }
    public string cosmosDbDatabaseId { get; set; }
    
    
    public string azStoreConnStr { get; set; }
    public string azStoreContName { get; set; }
    public string weeklyOrdersFile { get; set; }
    public string harvestFile { get; set; }
}