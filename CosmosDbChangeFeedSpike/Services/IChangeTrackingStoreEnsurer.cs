using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services
{
    public interface IChangeTrackingStoreEnsurer
    {
        Task EnsureStores();
    }
}
