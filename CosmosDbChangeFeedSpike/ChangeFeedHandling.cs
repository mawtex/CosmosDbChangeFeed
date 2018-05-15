using CosmosDbChangeFeedSpike.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike
{
    class DocumentFeedObserver : IChangeFeedObserver
    {
        IConfiguration _config;
        public DocumentFeedObserver()
        {
            _config = ServiceLocator.GetService<IConfiguration>();
        }

        public Task OpenAsync(ChangeFeedObserverContext context)
        {
            Console.WriteLine("Worker opened, {0}", context.PartitionKeyRangeId);
            return Task.CompletedTask;  // Requires targeting .NET 4.6+.
        }

        public Task CloseAsync(ChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            Console.WriteLine($"Worker closed, {context.PartitionKeyRangeId}, reason {reason}");


            return Task.CompletedTask;
        }

        public async Task ProcessChangesAsync(ChangeFeedObserverContext context, IReadOnlyList<Document> docs)
        {
            var sw = new Stopwatch();
            sw.Start();

            var documentHashCalculator = ServiceLocator.GetService<IDocumentHashCalculator>();

            var documentChanges = new List<DocumentChangeInfo>();

            foreach (var doc in docs)
            {
                int contentHash = documentHashCalculator.CalculateHash(doc);

                documentChanges.Add(new DocumentChangeInfo
                    {
                        PartitionKey = doc.GetPropertyValue<string>(_config.MonitoredCollectionPartitionKeyName),
                        Id = doc.Id,
                        ContentHash = contentHash
                });
            }

            var changeTracker = ServiceLocator.GetService<IChangeTracker>();
            await changeTracker.ProcessContentHash(documentChanges);

            Console.WriteLine($"Handled {docs.Count} documents in {sw.ElapsedMilliseconds} ms - avarage of {sw.ElapsedMilliseconds / docs.Count}");
        }

        private int CalculateContentHash(Document doc)
        {
            return doc.ToString().GetHashCode();
        }
    }

    class ChangeFeedHandling
    {
        public static async Task StartChangeFeedHost()
        {
            var config = ServiceLocator.GetService<IConfiguration>();

            string hostName = config.MonitoredCollectionName + "_monitor_" + Guid.NewGuid().ToString();
            DocumentCollectionInfo documentCollectionLocation = new DocumentCollectionInfo
            {
                Uri = config.DatabaseAccountUri,
                MasterKey = config.DatabaseAccountKey,
                DatabaseName = config.MonitoredDatabaseName,
                CollectionName = config.MonitoredCollectionName
            };
            DocumentCollectionInfo leaseCollectionLocation = new DocumentCollectionInfo
            {
                Uri = config.DatabaseAccountUri,
                MasterKey = config.DatabaseAccountKey,
                DatabaseName = config.ChangeTrackingDatabaseName,
                CollectionName = config.ChangeTrackingLeaseCollectionName
            };
            Console.WriteLine("Main program: Creating ChangeFeedEventHost...");

            ChangeFeedOptions feedOptions = new ChangeFeedOptions();
            ChangeFeedHostOptions feedHostOptions = new ChangeFeedHostOptions();



            ChangeFeedEventHost host = new ChangeFeedEventHost(hostName, documentCollectionLocation, leaseCollectionLocation, feedOptions, feedHostOptions);
            await host.RegisterObserverAsync<DocumentFeedObserver>();
            Console.WriteLine("Main program: press Enter to stop...");
            Console.ReadLine();
            await host.UnregisterObserversAsync();
        }
    }
}
