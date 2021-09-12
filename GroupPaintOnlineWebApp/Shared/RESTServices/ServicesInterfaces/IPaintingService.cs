using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.RESTServices.ServicesInterfaces
{
    public interface IPaintingService
    {
        Task<IEnumerable<Painting>> GetPaintings();
        Task<HttpResponseMessage> GetPainting(string id);
        Task<HttpResponseMessage> PostPainting(Painting painting);
        Task<HttpResponseMessage> PutPainting(string id, Painting painting);
        Task<HttpResponseMessage> DeletePainting(string id);
    }
}
