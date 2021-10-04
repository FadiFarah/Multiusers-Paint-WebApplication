using GroupPaintOnlineWebApp.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class PaintingsListBase : ComponentBase
    {
        [Inject]
        public HttpClient httpClient { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }

        public List<Painting> Paintings { get; set; }
        public string URL { get; set; }
        public string DisplayImage { get; set; }
        public string UserId { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                var response = await TokenProvider.RequestAccessToken();
                response.TryGetToken(out var token);
                if (token.Value != null)
                {
                    Console.WriteLine(token.Value);
                    httpClient.DefaultRequestHeaders.Add("Authorization", token.Value.ToString());
                    var handler = new JwtSecurityTokenHandler();
                    var userDetails = handler.ReadJwtToken(token.Value);
                    UserId = userDetails.Claims.First(claim => claim.Type == "oid").Value;
                }
                var paintingsResponse = await httpClient.GetFromJsonAsync<List<Painting>>("https://grouppaintonline-apim.azure-api.net/v1/painting/");
                Paintings = paintingsResponse.FindAll(s => s.ContributedUsers.Contains(UserId));
                DisplayImage = "none";
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();

            }
        }
        public void ViewImageClick(string imageURL)
        {
            URL = imageURL;
            DisplayImage = "block";
        }
        public void CloseImageView()
        {
            URL = "";
            DisplayImage = "none";
        }
        public async Task RemoveContributionPaintingClick(string Id)
        {

            var response = await httpClient.GetFromJsonAsync<Painting>("https://grouppaintonline-apim.azure-api.net/v1/painting/"+Id);
            if (response!=null)
            {
                if (response.ContributedUsers.Count > 1)
                {
                    response.ContributedUsers.Remove(UserId);
                    await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", response);
                }
                else
                {
                    await httpClient.DeleteAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/"+Id);
                }
                var paintingsResponse = await httpClient.GetFromJsonAsync<List<Painting>>("https://grouppaintonline-apim.azure-api.net/v1/painting");
                Paintings = paintingsResponse.FindAll(s => s.ContributedUsers.Contains(UserId));
                StateHasChanged();
            }
        }
        public async Task EnterOrRecreateRoom(Room roomDetails)
        {

            var response = await httpClient.GetAsync("https://grouppaintonline-apim.azure-api.net/api/room/" + roomDetails.id);
            if (response.Content.ReadAsByteArrayAsync().Result.Length==0)
            {
                var painting = Paintings.Find(paint => paint.id == roomDetails.id);
                painting.CreatorId = UserId;
                await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", painting);
                await httpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/api/room/", roomDetails);
                NavManager.NavigateTo("/room/" + roomDetails.id + "/" + roomDetails.Password,true);
            }
            else
            {
                Room room = await response.Content.ReadFromJsonAsync<Room>();
                if(room.CurrentUsers>=room.MaxUsers)
                    NavManager.NavigateTo("/paintingsList/", true);
                else
                    NavManager.NavigateTo("/room/" + roomDetails.id + "/" + roomDetails.Password, true);
            }
        }
    }
}
