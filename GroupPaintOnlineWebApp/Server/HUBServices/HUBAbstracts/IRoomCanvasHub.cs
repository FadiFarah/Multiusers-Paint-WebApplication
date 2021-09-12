using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.HUBServices.HUBAbstracts
{
    public abstract class IRoomCanvasHub : Hub
    {
        public abstract Task AddToGroup(string groupName, string connectionId);
        public abstract Task RemoveFromGroup(string groupName, string connectionId);
        public abstract Task SendContext(string imageURL, string roomId);
        public abstract Task SendChatMessage(string message, string roomId);
    }
}
