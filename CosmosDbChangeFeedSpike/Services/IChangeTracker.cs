using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services
{
    public interface IChangeTracker
    {
        Task ProcessContentHash(IReadOnlyCollection<DocumentChangeInfo> documentChanges);
    }
}