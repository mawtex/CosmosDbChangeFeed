using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services.Impl
{
    public class ChangeTracker : IChangeTracker
    {
        ICosmosDbAccess _cosmosDbAccess;
        IConfiguration _config;


        public ChangeTracker(ICosmosDbAccess cosmosDbAccess, IConfiguration config)
        {
            _cosmosDbAccess = cosmosDbAccess;
            _config = config;
        }


        public async Task ProcessContentHash(IReadOnlyCollection<DocumentChangeInfo> documentChanges)
        {
            var client = _cosmosDbAccess.GetDocumentClient();

            var changeList = new List<DocumentChangeInfo>(documentChanges);

            var partitionKeys = changeList.Select(f => f.PartitionKey).Distinct().ToList();

            foreach (var partitionKey in partitionKeys)
            {
                var idsInPartition = changeList.Where(g => g.PartitionKey == partitionKey).Select(g => g.Id).ToList();

                var lastRegisteredChangeQuery = client.CreateDocumentQuery<DocumentChangeInfo>(_cosmosDbAccess.GetChangeTrackerCollectionUri())
                    .Where(f => f.PartitionKey== partitionKey && idsInPartition.Contains(f.Id))
                    .AsDocumentQuery();

                if (lastRegisteredChangeQuery.HasMoreResults)
                {
                    foreach (DocumentChangeInfo lastRegisteredChange in await lastRegisteredChangeQuery.ExecuteNextAsync<DocumentChangeInfo>())
                    {
                        var currentChange = changeList.First(f => f.PartitionKey == partitionKey && f.Id == lastRegisteredChange.Id);

                        if (lastRegisteredChange.ContentHash != currentChange.ContentHash)
                        {
                            currentChange.Action = "update";
                            await client.UpsertDocumentAsync(_cosmosDbAccess.GetChangeTrackerCollectionUri(), currentChange);
                        }

                        changeList.Remove(currentChange);
                    }
                }
            }

            foreach (var change in changeList) // what is left in this list is new inserts
            {
                change.Action = "insert";
                await client.UpsertDocumentAsync(_cosmosDbAccess.GetChangeTrackerCollectionUri(), change);
            }
        }
    }
}
