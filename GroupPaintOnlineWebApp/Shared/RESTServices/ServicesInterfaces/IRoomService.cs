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
        Task<HttpResponseMessage> PostRoom(Room room);
        Task<HttpResponseMessage> PutRoom(string id, Room room);
        Task<HttpResponseMessage> DeleteRoom(string id);
    }
}
