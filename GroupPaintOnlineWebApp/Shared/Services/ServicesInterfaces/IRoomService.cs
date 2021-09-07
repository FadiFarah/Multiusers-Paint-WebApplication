using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRooms();
        Task<HttpResponseMessage> GetRoom(string id);
        Task<HttpResponseMessage> GetRoom(string id,string password);
    }
}
