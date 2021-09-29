using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace CosmosChangeFeedFunction
{
    public class RoomTriggerFunction
    {

        [FunctionName("RoomTriggerFunction")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "GroupPaintOnlineDb",
            collectionName: "Rooms",
            ConnectionStringSetting = "",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            [SignalR(HubName = "roomsListHub")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                foreach (var i in input)
                {
                    log.LogInformation("First document Id " + i.Id);
                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        Target = "roomsListUpdated",
                        Arguments = new[] { "" },
                    });
                }
                return;
            }
        }
    }
}
