using GroupPaintOnlineWebApp.Shared.HUBServices.HUBAbstracts;
using GroupPaintOnlineWebApp.Shared.Services;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using GroupPaintOnlineWebApp.Server.Data;
using GroupPaintOnlineWebApp.Server.Controllers;

namespace GroupPaintOnlineWebApp.Shared.HUBServices
{
    public class RoomCanvasHub : IRoomCanvasHub
    {
        private static Dictionary<string, List<string>> OnlineClientsInGroups= new Dictionary<string, List<string>>();

        public static RoomService roomService;

        private readonly RoomsController _roomsController;
        private readonly ApplicationDbContext _applicationDbContext;
        public RoomCanvasHub(ApplicationDbContext applicationDbContext, RoomsController roomsController)
        {
            _applicationDbContext = applicationDbContext;
            _roomsController = roomsController;
        }
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("User Connected");
            await Clients.Caller.SendAsync("UserConnected", Context.ConnectionId);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("User Disconnected");
            foreach (var item in OnlineClientsInGroups)
            {
                if (item.Value.Contains(Context.ConnectionId))
                {
                    var room = await _roomsController.GetRoom(item.Key);
                    if (room.Value != null)
                    {
                        if (room.Value.CurrentUsers <= 1)
                        {
                            await _roomsController.DeleteRoom(item.Key);
                            OnlineClientsInGroups.Remove(item.Key);
                            break;
                        }
                        else
                        {
                            room.Value.CurrentUsers -= 1;
                            await _roomsController.PutRoom(item.Key, room.Value);
                            item.Value.Remove(Context.ConnectionId);
                        }
                    }
                }
            }
                await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        }

        public override async Task AddToGroup(string roomId, string connectionId)
        {
            var room = await _roomsController.GetRoom(roomId);
            if (room.Value != null)
            {
                room.Value.CurrentUsers += 1;
                await _roomsController.PutRoom(roomId,room.Value);
                Console.WriteLine("User Added to Group");
            }
            if (!OnlineClientsInGroups.ContainsKey(roomId))
                OnlineClientsInGroups[roomId] = new List<string>();
            OnlineClientsInGroups[roomId].Add(connectionId);
            await Groups.AddToGroupAsync(connectionId, roomId);
        }

        public override async Task RemoveFromGroup(string roomId, string connectionId)
        {
            Console.WriteLine("User Removed from Group");
            await Groups.RemoveFromGroupAsync(connectionId, roomId);
        }

    }
}
