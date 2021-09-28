using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFunctions.Configurations
{
    public class CosmosDbConfiguration : ICosmosDbConfiguration
    {
        public string AccountEndpoint { get; set; }
        public string AccountKey { get; set; }
        public string DatabaseName { get; set; }
        public string RoomContainerName { get; set; }
        public string RoomContainerPartitionKeyPath { get; set; }
        public string PaintingContainerName { get; set; }
        public string PaintingContainerPartitionKeyPath { get; set; }
    }
}
