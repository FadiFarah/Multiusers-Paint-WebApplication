using GroupPaintOnlineWebApp.Shared;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
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
        private HubConnection Connection { get; set; }
        public string URL { get; set; }
        public string ConnectionStatus { get; set; }


        protected async override Task OnInitializedAsync()
        {
            Room = new Room();
            await ConnectToServer();
        }

        private async Task ConnectToServer()
        {
            URL = "https://localhost:44301";
            Connection = new HubConnectionBuilder().WithUrl(URL + "/roomsListHub").Build();
            await Connection.StartAsync();
            ConnectionStatus = "Connected!";
            Console.WriteLine(ConnectionStatus);

            Connection.Closed += async (s) =>
            {
                ConnectionStatus = "Disconnected";
                Console.WriteLine(ConnectionStatus);
                await Connection.StartAsync();
            };

        }

        public async Task HandleValidSubmit()
        {
            Console.WriteLine(Room.RoomName + " " + Room.IsPublic + " " + Room.Password);
            Room.Id = Guid.NewGuid().ToString();
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
