using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignalRRoomFunction.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SignalRRoomFunction
{
    public class RealtimeRoomFunction
    {
        static Dictionary<string, List<string>> OnlineClientsInGroups = new Dictionary<string, List<string>>();
        static HttpClient httpClient = new HttpClient();
        static string AccessToken;

        [FunctionName("negotiate")]
        public SignalRConnectionInfo Negotiate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "roomHub")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }



        [FunctionName("RoomConnectionsTrigger")]
        public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, [SignalR(HubName = "roomHub")] IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            JObject eventData = (JObject)eventGridEvent.Data;
            SignalRConnectionData data = eventData.ToObject<SignalRConnectionData>();
            if (eventGridEvent.Data == null)
            {
                log.LogInformation("eventgridevent.data is null");
            }
            if (eventData == null)
            {
                log.LogInformation("event data is null");
            }
            if (data == null)
            {
                log.LogInformation("data is null");
            }
            if (eventGridEvent.EventType == "Microsoft.SignalRService.ClientConnectionConnected")
            {
                log.LogInformation("User connected");
                await signalRMessages.AddAsync(new SignalRMessage
                {
                    Target = "UserConnected",
                    Arguments = new[] { data.ConnectionId },
                });
            }
            else
            {
                httpClient.DefaultRequestHeaders.Remove("Authorization");
                httpClient.DefaultRequestHeaders.Add("Authorization", AccessToken);
                foreach (var item in OnlineClientsInGroups)
                {
                    if (item.Value.Contains(data.ConnectionId))
                    {
                        Room room = await httpClient.GetFromJsonAsync<Room>("https://grouppaintonline-apim.azure-api.net/api/room/" + item.Key);
                        if (room != null)
                        {
                            log.LogInformation(room.RoomName);
                            room.CurrentUsers -= 1;
                            if (room.CurrentUsers > 0)
                                await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/api/room/", room);
                            else
                                await httpClient.DeleteAsync("https://grouppaintonline-apim.azure-api.net/api/room/" + room.id);
                            item.Value.Remove(data.ConnectionId);
                        }
                    }
                }
                httpClient.DefaultRequestHeaders.Remove("Authorization");
                await signalRMessages.AddAsync(new SignalRMessage
                {
                    Target = "UserDisconnected",
                    Arguments = new[] { data.ConnectionId },
                });
            }
            log.LogInformation(eventGridEvent.EventType);
            log.LogInformation(eventGridEvent.Data.ToString());
        }

        [FunctionName("addtogroup")]
        public async Task AddToGroup([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        [SignalR(HubName = "roomHub")]
        IAsyncCollector<SignalRGroupAction> signalRGroupActions, ILogger logger)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BodyDetails bodyDetails = JsonConvert.DeserializeObject<BodyDetails>(requestBody);

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", bodyDetails.AccessToken);
            AccessToken = bodyDetails.AccessToken;
            Room room = await httpClient.GetFromJsonAsync<Room>("https://grouppaintonline-apim.azure-api.net/api/room/" + bodyDetails.GroupName);

            if (room != null)
            {
                room.CurrentUsers += 1;
                await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/api/room/", room);
                Console.WriteLine("User Added to Group");
            }
            if (!OnlineClientsInGroups.ContainsKey(bodyDetails.GroupName))
                OnlineClientsInGroups[bodyDetails.GroupName] = new List<string>();
            OnlineClientsInGroups[bodyDetails.GroupName].Add(bodyDetails.ConnectionId);

            httpClient.DefaultRequestHeaders.Remove("Authorization");

            await signalRGroupActions.AddAsync(
                new SignalRGroupAction
                {
                    ConnectionId = bodyDetails.ConnectionId,
                    GroupName = bodyDetails.GroupName,
                    Action = GroupAction.Add
                });
        }

        [FunctionName("sendpaintingupdates")]
        public async Task SendPaintingUpdates([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
        [SignalR(HubName = "roomHub")]
        IAsyncCollector<SignalRMessage> signalRMessages, ILogger logger)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Painting painting = JsonConvert.DeserializeObject<Painting>(requestBody);
            logger.LogInformation("Painting id " + painting.id + "has been updated");

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "paintingUpdated",
                    Arguments = new[] { painting },
                    GroupName = painting.id,
                });
        }
        [FunctionName("sendchatmessage")]
        public async Task SendChatMessage([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        [SignalR(HubName = "roomHub")]
        IAsyncCollector<SignalRMessage> signalRMessages, ILogger logger)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BodyDetails bodyDetails = JsonConvert.DeserializeObject<BodyDetails>(requestBody);
            logger.LogInformation(bodyDetails.ConnectionId + " sent a message to group id " + bodyDetails.GroupName);

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "newChatMessage",
                    Arguments = new[] { bodyDetails.Message },
                    GroupName = bodyDetails.GroupName,
                });
        }

        [FunctionName("sendcontributionrequest")]
        public async Task SendContributionRequest([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        [SignalR(HubName = "roomHub")]
        IAsyncCollector<SignalRMessage> signalRMessages, ILogger logger)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BodyDetails bodyDetails = JsonConvert.DeserializeObject<BodyDetails>(requestBody);
            logger.LogInformation(bodyDetails.ConnectionId + " sent a contribution request to group id " + bodyDetails.GroupName);

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "contributionRequest",
                    Arguments = new[] { bodyDetails.Name, bodyDetails.UserId },
                    GroupName = bodyDetails.GroupName,
                });
        }

        [FunctionName("sendcontributionresponse")]
        public async Task SendContributionResponse([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        [SignalR(HubName = "roomHub")]
        IAsyncCollector<SignalRMessage> signalRMessages, ILogger logger)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            BodyDetails bodyDetails = JsonConvert.DeserializeObject<BodyDetails>(requestBody);
            logger.LogInformation(bodyDetails.ConnectionId + " sent a contribution request to group id " + bodyDetails.GroupName);

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "contributionResponse",
                    Arguments = new[] { bodyDetails.UserId, bodyDetails.IsAccepted },
                    GroupName = bodyDetails.GroupName,
                });
        }

    }

}