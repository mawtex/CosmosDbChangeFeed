using CosmosDbChangeFeedSpike.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike
{
    class Program
    {
        static void Main(string[] args)
        {
            Services.Impl._registrator.RegisterServices();

            var ensurer = ServiceLocator.GetService<IChangeTrackingStoreEnsurer>();
            Task ensure = ensurer.EnsureStores();
            ensure.Wait();

            Task palle = Palle();
            palle.Wait();

            Task t = ChangeFeedHandling.StartChangeFeedHost();
            t.Wait();

            Console.ReadKey();
            Console.ReadKey();
        }

        static async Task Palle()
        {
            var config = ServiceLocator.GetService<IConfiguration>();
            var dbAccess = ServiceLocator.GetService<ICosmosDbAccess>();
            var client = dbAccess.GetDocumentClient();

            var partitionKeys = new List<string>();

            var lastRegisteredChangeQuery = client.CreateDocumentQuery<DocumentChangeInfo>(dbAccess.GetChangeTrackerCollectionUri(),
                new FeedOptions { EnableCrossPartitionQuery = true, MaxItemCount = 50000 })
                .AsDocumentQuery();

            int counter = 0;

            if (lastRegisteredChangeQuery.HasMoreResults)
            {
                foreach (DocumentChangeInfo lastRegisteredChange in await lastRegisteredChangeQuery.ExecuteNextAsync<DocumentChangeInfo>())
                {
                    if (!partitionKeys.Contains(lastRegisteredChange.PartitionKey))
                    {
                        partitionKeys.Add(lastRegisteredChange.PartitionKey);
                    }
                    counter++;
                }
            }

            foreach (var item in partitionKeys)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine($"--- read a total of {counter} items");

            Console.ReadKey();
        }
    }
}
    