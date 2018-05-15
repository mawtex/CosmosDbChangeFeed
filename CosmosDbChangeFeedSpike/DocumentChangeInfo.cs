using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDbChangeFeedSpike
{
    public class DocumentChangeInfo
    {
        public string PartitionKey { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public int ContentHash { get; set; }
        public string Action { get; set; }
    }
}
