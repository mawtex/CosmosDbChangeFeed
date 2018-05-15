using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike.Services
{
    public interface IDocumentHashCalculator
    {
        int CalculateHash(Document doc);

    }
}
