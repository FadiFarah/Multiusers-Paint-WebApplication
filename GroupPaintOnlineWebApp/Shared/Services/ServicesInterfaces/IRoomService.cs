using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<Room>> GetRooms();
    }
}
