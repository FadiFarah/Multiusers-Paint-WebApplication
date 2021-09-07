using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.Services
{
    public class RoomService:IRoomService
    {
        private readonly HttpClient httpClient;
        public RoomService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Room>> GetRooms()
        {
            return await httpClient.GetFromJsonAsync<Room[]>("/api/Rooms");
        }

    }
}
