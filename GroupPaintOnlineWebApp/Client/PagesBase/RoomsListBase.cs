using GroupPaintOnlineWebApp.Shared;
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
        public string URL { get; set; }
        public string ConnectionStatus { get; set; }

        private HubConnection Connection { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                Rooms = (await RoomService.GetRooms()).ToArray();
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
            URL = "https://localhost:44301";
            Connection = new HubConnectionBuilder().WithUrl(URL+"/roomsListHub").Build();
            await Connection.StartAsync();
            ConnectionStatus = "Connected!";
            Console.WriteLine(ConnectionStatus);


            Connection.On("RoomCreation", async () =>
            {
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

            Connection.On("RoomUpdation", async () =>
            {
                Rooms = (await RoomService.GetRooms()).ToArray();
                StateHasChanged();
            });

            Connection.On("RoomDeletion", async () =>
            {
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

            Connection.Closed += async (s) =>
            {
                ConnectionStatus = "Disconnected";
                Console.WriteLine(ConnectionStatus);
                await Connection.StartAsync();
            };

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
