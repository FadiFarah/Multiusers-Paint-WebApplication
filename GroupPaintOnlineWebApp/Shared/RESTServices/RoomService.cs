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

        public async Task<HttpResponseMessage> GetRoom(string id)
        {
            return await httpClient.GetAsync("/api/Rooms/"+id);
        }

        public async Task<HttpResponseMessage> PostRoom(Room room)
        {
            return await httpClient.PostAsJsonAsync("api/Rooms",room);
        }

        public async Task<HttpResponseMessage> PutRoom(string id, Room room)
        {
            return await httpClient.PutAsJsonAsync("api/Rooms/"+id, room);
        }

        public async Task<HttpResponseMessage> DeleteRoom(string id)
        {
            return await httpClient.DeleteAsync("api/Rooms/"+id);
        }
    }
}
