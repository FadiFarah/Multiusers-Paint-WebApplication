using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFunctions
{
    public interface ICosmosDbConfiguration
    {
        string AccountEndpoint { get; set; }
        string AccountKey { get; set; }
        string DatabaseName { get; set; }
        string RoomContainerName { get; set; }
        string RoomContainerPartitionKeyPath { get; set; }
        string PaintingContainerName { get; set; }
        string PaintingContainerPartitionKeyPath { get; set; }
    }
}
