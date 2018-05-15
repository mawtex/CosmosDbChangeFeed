using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services.Impl
{
    public class ChangeTrackingStoreEnsurer : IChangeTrackingStoreEnsurer
    {
        IConfiguration _config;

        public ChangeTrackingStoreEnsurer(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnsureStores()
        {
            var client = new DocumentClient(_config.DatabaseAccountUri, _config.DatabaseAccountKey);

            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = _config.ChangeTrackingDatabaseName });
            Uri changeTrackingDatabaseUri = UriFactory.CreateDatabaseUri(_config.ChangeTrackingDatabaseName);
            await client.CreateDocumentCollectionIfNotExistsAsync(changeTrackingDatabaseUri, new DocumentCollection { Id = _config.ChangeTrackingLeaseCollectionName });
            await client.CreateDocumentCollectionIfNotExistsAsync(changeTrackingDatabaseUri, new DocumentCollection { Id = _config.ChangeTrackingDataCollectionName });
        }
    }
}
