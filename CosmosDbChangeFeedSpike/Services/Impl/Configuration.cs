using System;

namespace CosmosDbChangeFeedSpike.Services.Impl
{
    public class Configuration : IConfiguration
    {
        public Uri DatabaseAccountUri => new Uri("https://localhost:8081/");

        public string DatabaseAccountKey => "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        public bool DatabaseDirectTcpConnectionMode => true;

        public string ChangeTrackingDatabaseName => "ChangeTracking";

        public string ChangeTrackingDataCollectionName => "ProductChangeTracker";

        public string ChangeTrackingLeaseCollectionName => "Leases";

        public string MonitoredDatabaseName => "Products";

        public string MonitoredCollectionName => "Products";

        public string MonitoredCollectionPartitionKeyName => "catalogId";

    }
}
