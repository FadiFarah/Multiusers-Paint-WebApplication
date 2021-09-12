using GroupPaintOnlineWebApp.Shared.RESTServices.ServicesInterfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.RESTServices
{
    public class PaintingService : IPaintingService
    {
        private readonly HttpClient httpClient;
        public PaintingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<Painting>> GetPaintings()
        {
            return await httpClient.GetFromJsonAsync<Painting[]>("/api/Paintings");
        }

        public async Task<HttpResponseMessage> GetPainting(string id)
        {
            return await httpClient.GetAsync("/api/Paintings/" + id);
        }

        public async Task<HttpResponseMessage> PostPainting(Painting painting)
        {
            return await httpClient.PostAsJsonAsync("api/Paintings", painting);
        }

        public async Task<HttpResponseMessage> PutPainting(string id, Painting painting)
        {
            return await httpClient.PutAsJsonAsync("api/Paintings/" + id, painting);
        }

        public async Task<HttpResponseMessage> DeletePainting(string id)
        {
            return await httpClient.DeleteAsync("api/Paintings/" + id);
        }
    }
}
