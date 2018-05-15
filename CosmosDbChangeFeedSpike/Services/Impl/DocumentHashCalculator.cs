using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace CosmosDbChangeFeedSpike.Services.Impl
{
    public class DocumentHashCalculator : IDocumentHashCalculator
    {
        private static readonly string[] _nonDataFields = "_etag,_ts,_lsn".Split(',');


        public int CalculateHash(Document doc)
        {
            foreach (var nonDataField in _nonDataFields)
            {
                doc.SetPropertyValue(nonDataField, null);
            }

            Random rnd = new Random();
            if (rnd.Next(0, 2)==0                )
            {
                doc.SetPropertyValue("RandomCHange", true);
            }
            

            return doc.ToString().GetHashCode();
        }
    }
}
