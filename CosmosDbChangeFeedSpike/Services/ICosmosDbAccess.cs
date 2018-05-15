using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services
{
    public interface ICosmosDbAccess
    {
        DocumentClient GetDocumentClient();
        Uri GetChangeTrackerCollectionUri();
    }
}
