using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using GroupPaintOnlineWebApp.Client.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class RoomsListBase : ComponentBase
    {
        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        public HttpClient httpClient { get; set; }
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
                await GetRooms();
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
                        RoomsList[i].id = Rooms[i].id;
                        RoomsList[i].IsPublic = Rooms[i].IsPublic;
                    }
                }
                StateHasChanged();
            }
        }

        private async Task ConnectToServer()
        {
            URL = "https://grouppaintonline-roomslisthubfunction.azurewebsites.net/api";
            Connection = new HubConnectionBuilder().WithUrl(URL).Build();
            await Connection.StartAsync();
            ConnectionStatus = "Connected!";
            Console.WriteLine(ConnectionStatus);

            Connection.On<string>("roomsListUpdated", async (value) =>
            {
                Rooms = await httpClient.GetFromJsonAsync<Room[]>("https://grouppaintonline-apim.azure-api.net/api/room");
                if (Rooms != null)
                {
                    RoomsList = new Room[Rooms.Length];
                    for (int i = 0; i < RoomsList.Length; i++)
                    {
                        RoomsList[i] = new Room();
                        RoomsList[i].id = Rooms[i].id;
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
        private async Task GetRooms()
        {
            var response = await TokenProvider.RequestAccessToken();
            response.TryGetToken(out var token);
            if (token.Value != null)
            {
                Console.WriteLine(token.Value);
                httpClient.DefaultRequestHeaders.Add("Authorization", token.Value.ToString());
                Rooms = await httpClient.GetFromJsonAsync<Room[]>("https://grouppaintonline-apim.azure-api.net/api/room");
            }
        }
        protected async Task FormSubmitted(EditContext editContext)
        {
            if (editContext.Model is Room)
            {
                Room returnedRoom = (Room)editContext.Model;

                Room room = await httpClient.GetFromJsonAsync<Room>("https://grouppaintonline-apim.azure-api.net/api/room/" + returnedRoom.id);
                if (room == null || room.Password != returnedRoom.Password)
                {
                    NavManager.NavigateTo("/roomslist");
                    return;
                }

                if (room.Password == returnedRoom.Password && room.IsPublic == false)
                {
                    NavManager.NavigateTo("/room/" + returnedRoom.id + "/" + returnedRoom.Password, true);
                    return;
                }
                NavManager.NavigateTo("/room/" + returnedRoom.id, true);
            }

        }
        public void CreateNewButton()
        {
            NavManager.NavigateTo("/createroom",true);
        }

    }
}
