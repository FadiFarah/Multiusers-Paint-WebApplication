using GroupPaintOnlineWebApp.Shared;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class CreateRoomBase : ComponentBase
    {

        [Inject]
        public IRoomService RoomService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public Room Room { get; set; }
        protected override void OnInitialized()
        {
            Room = new Room();
        }
        public async Task HandleValidSubmit()
        {
            Console.WriteLine(Room.RoomName + " " + Room.IsPublic + " " + Room.Password);
            Room.Id = Guid.NewGuid().ToString();
            Room.CurrentUsers = 1;
            var response = await RoomService.PostRoom(Room);
            if (response.StatusCode == HttpStatusCode.Created)
            {
                NavManager.NavigateTo("/room/" + Room.Id + "/" + Room.Password);
            }
            else
            {
                NavManager.NavigateTo("/createroom");
            }
        }
    }
}
