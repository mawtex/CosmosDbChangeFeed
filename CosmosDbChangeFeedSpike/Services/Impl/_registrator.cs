using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services.Impl
{
    internal static class _registrator
    {
        internal static void RegisterServices()
        {
            ServiceLocator.AddSingleton(typeof(IConfiguration), typeof(Configuration));
            ServiceLocator.AddSingleton(typeof(ICosmosDbAccess), typeof(CosmosDbAccess));
            ServiceLocator.AddSingleton(typeof(IChangeTracker), typeof(ChangeTracker));
            ServiceLocator.AddSingleton(typeof(IDocumentHashCalculator), typeof(DocumentHashCalculator));

            ServiceLocator.AddTransient(typeof(IChangeTrackingStoreEnsurer), typeof(ChangeTrackingStoreEnsurer));
        }
    }
}
