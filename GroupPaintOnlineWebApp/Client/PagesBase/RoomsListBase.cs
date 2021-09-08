using GroupPaintOnlineWebApp.Shared;
using GroupPaintOnlineWebApp.Shared.HUBServices;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class RoomsListBase :ComponentBase
    {
        [Inject]
        public IRoomService RoomService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }

        public Room[] Rooms { get; set; }
        public Room[] RoomsList { get; set; }

        private HubConnection Connection { get; set; }
        private RoomsListHub RoomsListHub { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Rooms = (await RoomService.GetRooms()).ToArray();
                RoomsListHub = new RoomsListHub();
                await ConnectToServer();
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();

            }
            finally
            {
                if (Rooms != null)
                {
                    RoomsList = new Room[Rooms.Length];
                    for (int i = 0; i < RoomsList.Length; i++)
                    {
                        RoomsList[i] = new Room();
                        RoomsList[i].Id = Rooms[i].Id;
                        RoomsList[i].IsPublic = Rooms[i].IsPublic;
                    }
                }
            }
        }

        private async Task ConnectToServer()
        {
            Connection = new HubConnectionBuilder().WithUrl(RoomsListHub.URL + "/roomsListHub").Build();
            await Connection.StartAsync();
            RoomsListHub.ConnectionStatus = "Connected!";
            Console.WriteLine(RoomsListHub.ConnectionStatus);

            Connection.Closed += async (s) =>
            {
                RoomsListHub.ConnectionStatus = "Disconnected";
                Console.WriteLine(RoomsListHub.ConnectionStatus);
                await Connection.StartAsync();
            };

            Connection.On<Room>("RoomCreation", async r =>
            {
                Console.WriteLine("OnRoomCreation: " + r.RoomName);
                Rooms = (await RoomService.GetRooms()).ToArray();
                if (Rooms != null)
                {
                    RoomsList = new Room[Rooms.Length];
                    for (int i = 0; i < RoomsList.Length; i++)
                    {
                        RoomsList[i] = new Room();
                        RoomsList[i].Id = Rooms[i].Id;
                        RoomsList[i].IsPublic = Rooms[i].IsPublic;
                    }
                }
                StateHasChanged();
            });

        }

        protected async Task FormSubmitted(EditContext editContext)
        {
            if (editContext.Model is Room)
            {
                Room r = ((Room)editContext.Model);
                if (r.IsPublic)
                {
                    NavManager.NavigateTo("/room/" + r.Id);
                }
                else
                {
                    var response = await RoomService.GetRoom(r.Id,r.Password);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        NavManager.NavigateTo("/roomslist");
                    }
                    else
                    {
                        NavManager.NavigateTo("/room/" + r.Id + "/" + r.Password);
                    }
                }
            }


        }

    }
}
