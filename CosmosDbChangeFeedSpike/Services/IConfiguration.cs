using System;

namespace CosmosDbChangeFeedSpike.Services
{
    public interface IConfiguration
    {
        // global database collection info
        Uri DatabaseAccountUri { get; }
        string DatabaseAccountKey { get; }
        bool DatabaseDirectTcpConnectionMode { get; }

        // tracking specific info 
        string ChangeTrackingDatabaseName { get; }
        string ChangeTrackingLeaseCollectionName { get; }
        string ChangeTrackingDataCollectionName { get; }

        // source data info
        string MonitoredDatabaseName { get; }
        string MonitoredCollectionName { get; }
        string MonitoredCollectionPartitionKeyName { get; }
    }
}
