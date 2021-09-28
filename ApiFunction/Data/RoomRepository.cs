using ApiFunctions.Configurations;
using ApiFunctions.Models;
using ApiFunctions.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiFunctions.Data
{
    public class RoomRepository : CosmosDbService<Room>
    {
        public RoomRepository(IOptions<CosmosDbConfiguration> cosmosDbConfiguration, CosmosClient client) : base(cosmosDbConfiguration,client)
        {

        }
        public override string ContainerName => _cosmosDbConfiguration.RoomContainerName;
    }
}
