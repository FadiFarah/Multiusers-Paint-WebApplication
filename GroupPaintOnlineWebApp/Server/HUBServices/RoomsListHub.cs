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
        public async override Task RoomCreated()
        {
            await Clients.All.SendAsync("RoomCreation");
            Console.WriteLine("RoomCreated");
        }

        public async override Task RoomUpdated()
        {
            await Clients.All.SendAsync("RoomUpdation");
            Console.WriteLine("RoomUpdated");
        }
        public async override Task RoomDeleted()
        {
            await Clients.All.SendAsync("RoomDeletion");
            Console.WriteLine("RoomDeleted");
        }
    }
}
