using GroupPaintOnlineWebApp.Shared.HUBServices.HUBAbstracts;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.HUBServices
{
    public class RoomsListHub : IRoomsListHub
    {
        public string URL { get; }
        public string ConnectionStatus { get; set; }
        public RoomsListHub()
        {
            URL = "https://localhost:44301";
            ConnectionStatus = "Closed";
        }
        public async override Task RoomCreated(Room room)
        {
            await Clients.All.SendAsync("RoomCreation", room);
            Console.WriteLine("RoomCreated");
        }
    }
}
