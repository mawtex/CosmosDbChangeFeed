using System;
using Microsoft.Azure.Documents.Client;

namespace CosmosDbChangeFeedSpike.Services.Impl
{
    public class CosmosDbAccess : ICosmosDbAccess
    {
        IConfiguration _config;

        DocumentClient _documentClient = null;

        public CosmosDbAccess(IConfiguration config)
        {
            _config = config;

            if (_config.DatabaseDirectTcpConnectionMode)
            {
                ConnectionPolicy connectopnPolicy = new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp };
                _documentClient = new DocumentClient(_config.DatabaseAccountUri, _config.DatabaseAccountKey, connectopnPolicy);
            }
            else
            {
                _documentClient = new DocumentClient(_config.DatabaseAccountUri, _config.DatabaseAccountKey);
            }
        }
        public DocumentClient GetDocumentClient()
        {
            return _documentClient;
        }

        public Uri GetChangeTrackerCollectionUri()
        {
            return UriFactory.CreateDocumentCollectionUri(_config.ChangeTrackingDatabaseName, _config.ChangeTrackingDataCollectionName);
        }

    }
}
