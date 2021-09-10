using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.HUBServices.HUBAbstracts
{
    public abstract class IRoomsListHub : Hub
    {
        public abstract Task RoomCreated();
        public abstract Task RoomUpdated();
        public abstract Task RoomDeleted();
    }
}
