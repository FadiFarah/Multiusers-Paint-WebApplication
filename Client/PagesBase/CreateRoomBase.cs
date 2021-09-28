using GroupPaintOnlineWebApp.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Authentication.WebAssembly.Msal.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Collections;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class CreateRoomBase : ComponentBase
    {
        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        HttpClient HttpClient { get; set; }
        [Inject]
        NavigationManager NavManager { get; set; }

        public Room Room { get; set; }
        public Painting Painting { get; set; }
        public string URL { get; set; }
        public string ConnectionStatus { get; set; }
        public string UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Room = new Room();
            var response = await TokenProvider.RequestAccessToken();
            response.TryGetToken(out var token);
            if (token.Value != null)
            {
                Console.WriteLine(token.Value);
                HttpClient.DefaultRequestHeaders.Add("Authorization", token.Value.ToString());
                var handler = new JwtSecurityTokenHandler();
                var userDetails = handler.ReadJwtToken(token.Value);
                UserId = userDetails.Claims.First(claim => claim.Type == "oid").Value;
            }
        }

        public async Task HandleValidSubmit()
        {
            Console.WriteLine(Room.RoomName + " " + Room.IsPublic + " " + Room.Password);
            Room.id = Guid.NewGuid().ToString();
            await HttpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/api/room/", Room);

            Painting = new Painting
            {
                id = Room.id,
                ContributedUsers = new List<string> { UserId },
                CreatorId = UserId,
                PaintingName = Room.RoomName,
                RoomDetails = Room,
                ImageURL = "",
                ShapesDetails = new ArrayList()
            };
            var paintingResponse = await HttpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", Painting);
            if (paintingResponse.IsSuccessStatusCode)
            {
                NavManager.NavigateTo("/room/" + Room.id + "/" + Room.Password, true);
            }

            else
            {
                NavManager.NavigateTo("/createroom");
            }
        }

    }
}
