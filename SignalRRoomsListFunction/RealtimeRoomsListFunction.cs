using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SignalRRoomsListFunction
{
    public class RealtimeRoomsListFunction
    {
        [FunctionName("negotiate")]
        public SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "roomsListHub")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }


        [FunctionName("roomscountchanged")]
        public static async Task RoomDeleted([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        [SignalR( HubName = "roomsListHub")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger logger)
        {
            logger.LogInformation("A rooms has been added/deleted");

            await signalRMessages.AddAsync(new SignalRMessage
            {
                Target = "roomsListUpdated",
                Arguments = new[] { "" },
            });
        }
    }
}