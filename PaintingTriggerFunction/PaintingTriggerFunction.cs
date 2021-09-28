using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace PaintingTriggerFunction
{
    public static class PaintingTriggerFunction
    {
        [FunctionName("PaintingTriggerFunction")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "GroupPaintOnlineDb",
            collectionName: "Paintings",
            ConnectionStringSetting = "",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, [SignalR(HubName = "roomHub")]
        IAsyncCollector<SignalRMessage> signalRMessages, ILogger logger)
        {
            if (input != null && input.Count > 0)
            {
                logger.LogInformation("Documents modified " + input.Count);
                logger.LogInformation("First document Id " + input[0].Id);

                    await signalRMessages.AddAsync(new SignalRMessage
                    {
                        Target="paintingUpdated",
                        GroupName = input[0].Id,
                        Arguments = new[] { input[0].Id },
                    });
            }
            return;
        }
    }
}
