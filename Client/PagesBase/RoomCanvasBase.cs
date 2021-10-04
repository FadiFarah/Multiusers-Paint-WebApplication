using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http;
using GroupPaintOnlineWebApp.Client.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Collections;
using Microsoft.AspNetCore.Http;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class RoomCanvasBase : ComponentBase
    {
        //Injections
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        public HttpClient httpClient { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        IConfiguration Config { get; set; }

        public string UserId { get; set; }

        //RouteUri Parameters
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string Password { get; set; }

        //Hub Connection Related Properties
        private HubConnection Connection { get; set; }
        public string URL { get; set; }
        public string ConnectionStatus { get; set; }

        //Chat Related Properties
        public List<string> ChatMessages { get; set; }
        public string ChatInput { get; set; }


        //Canvas Related Properties
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsPainting { get; set; }
        public string ImageURL { get; set; }
        public string DisplayBlockScreen = "none";
        public string DisplayChatBox = "none";
        public string DisplayContributeModal = "none";
        public string DisplayClearModal = "none";
        public string DisplayShareModal = "none";
        public string DisplayContributionRequestModal = "none";
        public string DisplayLoadingModal = "block";
        public string ContributeState { get; set; }
        public string SavingState { get; set; }
        public bool IsContributed { get; set; }
        public string ReceiverEmailInput { get; set; }

        public Room Room { get; set; }
        public Painting Painting { get; set; }
        public BodyDetails BodyDetails { get; set; }
        public Dictionary<string, string> ContributionRequesters { get; set; }

        protected override async Task OnInitializedAsync()
        {
            BodyDetails = new();
            ContributionRequesters = new();
            var response = await TokenProvider.RequestAccessToken();
            response.TryGetToken(out var token);
            if (token.Value != null)
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token.Value.ToString());
                Console.WriteLine(token.Value);
                BodyDetails.AccessToken = token.Value;
                var handler = new JwtSecurityTokenHandler();
                var userDetails = handler.ReadJwtToken(BodyDetails.AccessToken);
                UserId = userDetails.Claims.First(claim => claim.Type == "oid").Value;
            }


            Room = await httpClient.GetFromJsonAsync<Room>("https://grouppaintonline-apim.azure-api.net/api/room/" + Id);
            if (Room == null || Room.Password != Password || Room.CurrentUsers >= Room.MaxUsers)
            {
                NavManager.NavigateTo("/roomslist");
            }
            else
            {
                Painting = await httpClient.GetFromJsonAsync<Painting>("https://grouppaintonline-apim.azure-api.net/v1/painting/" + Id);
                ChatMessages = new();
                DisplayChatBox = "none";
                DisplayContributeModal = "none";
                DisplayClearModal = "none";
                var dimension = await JsRuntime.InvokeAsync<WindowDimension>("getWindowDimensions");
                await JsRuntime.InvokeVoidAsync("onInitialized");
                Height = dimension.Height;
                Width = dimension.Width;

                if (Painting.ContributedUsers.Contains(UserId))
                {
                    IsContributed = true;
                    DisplayBlockScreen = "none";
                    ContributeState = "CONTRIBUTED";
                    SavingState = "";
                }
                else
                {
                    IsContributed = false;
                    DisplayBlockScreen = "block";
                    ContributeState = "CONTRIBUTE";
                    SavingState = "contribution is needed to enable automatic saving...";
                }
                StateHasChanged();
                await ConnectToServer();
                DisplayLoadingModal = "none";

            }
        }


        private async Task ConnectToServer()
        {
            URL = "https://grouppaintonline-roomhubfunction.azurewebsites.net/api";
            Connection = new HubConnectionBuilder().WithUrl(URL).Build();
            await Connection.StartAsync();

            ConnectionStatus = "Connected!";
            Connection.Closed += async (a) =>
            {
                Console.WriteLine("Disconnected");
                NavManager.NavigateTo("https://localhost:44371", true);
            };
            Connection.On<string>("UserConnected", async (connectionId) =>
            {
                if (Connection.ConnectionId == connectionId)
                {
                    BodyDetails.ConnectionId = connectionId;
                    BodyDetails.GroupName = Id;
                    Console.WriteLine(connectionId + " has Connected");
                    await JsRuntime.InvokeAsync<ArrayList>("updateAndDrawShapes", Painting.ShapesDetails);
                    await httpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/hub2/addtogroup", BodyDetails);
                }
            });
            Connection.On<string>("UserDisconnected", (connectionId) =>
            {
                Console.WriteLine("User Disconnected");
            });


            Connection.On<Painting>("paintingUpdated", async (returnedPainting) =>
            {
                Painting = returnedPainting;
                await JsRuntime.InvokeAsync<string>("updateAndDrawShapes", Painting.ShapesDetails);
                if (Painting.ContributedUsers.Contains(UserId))
                {
                    SavingState = "changes saved.";
                    StateHasChanged();
                }

            });

            Connection.On<string, string>("contributionRequest", (name, userId) =>
            {
                if (Painting.CreatorId == UserId)
                {
                    ContributionRequesters[userId] = name;
                    DisplayContributionRequestModal = "block";
                    StateHasChanged();
                }
            });

            Connection.On<string, string>("contributionResponse", async (userId, isAccepted) =>
            {
                if (userId == UserId)
                {
                    if (isAccepted == "true")
                    {
                        DisplayBlockScreen = "none";
                        IsContributed = true;
                        ContributeState = "CONTRIBUTED";
                        Painting.ContributedUsers.Add(UserId);
                        await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", Painting);
                        StateHasChanged();
                        return;
                    }
                    IsContributed = false;
                    ContributeState = "CONTRIBUTE";
                    StateHasChanged();
                }
            });

            Connection.On<string>("newChatMessage", (message) =>
            {
                ChatMessages.Add(message);
            });
        }


        public void CanvasOnMouseDown()
        {
            IsPainting = true;
            if (Painting.ContributedUsers.Contains(UserId))
            {
                SavingState = "saving...";
            }
        }

        public async Task CanvasOnMouseUp()
        {
            if (IsPainting)
            {
                Painting.ImageURL = await JsRuntime.InvokeAsync<string>("getNewImage");
                Painting.ShapesDetails = await JsRuntime.InvokeAsync<ArrayList>("getShapesList");
                IsPainting = false;
                if (Painting.ContributedUsers.Contains(UserId))
                {
                    BodyDetails.ConnectionId = Connection.ConnectionId;
                    BodyDetails.GroupName = Id;
                    await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", Painting);
                    SavingState = "changes saved.";
                }
            }
            return;

        }
        public async Task CanvasOnMouseOut()
        {
            if (IsPainting)
            {
                Painting.ImageURL = await JsRuntime.InvokeAsync<string>("getNewImage");
                Painting.ShapesDetails = await JsRuntime.InvokeAsync<ArrayList>("getShapesList");
                IsPainting = false;
                if (Painting.ContributedUsers.Contains(UserId))
                {
                    BodyDetails.ConnectionId = Connection.ConnectionId;
                    BodyDetails.GroupName = Id;
                    await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", Painting);
                    SavingState = "changes saved.";
                }
            }
            return;
        }
        public async Task UndoClick()
        {
            await JsRuntime.InvokeVoidAsync("undoDraw");
            Painting.ImageURL = await JsRuntime.InvokeAsync<string>("getNewImage");
            Painting.ShapesDetails = await JsRuntime.InvokeAsync<ArrayList>("getShapesList");
            if (Painting.ContributedUsers.Contains(UserId))
                await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", Painting);
            return;
        }
        public async Task RedoClick()
        {
            await JsRuntime.InvokeVoidAsync("redoDraw");
            Painting.ImageURL = await JsRuntime.InvokeAsync<string>("getNewImage");
            Painting.ShapesDetails = await JsRuntime.InvokeAsync<ArrayList>("getShapesList");
            if (Painting.ContributedUsers.Contains(UserId))
                await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", Painting);
            return;
        }
        public void ChatButtonClick()
        {
            if (DisplayChatBox == "none")
                DisplayChatBox = "block";
            else
                DisplayChatBox = "none";
        }
        public void CloseChatBox()
        {
            DisplayChatBox = "none";
        }
        public async Task OnEnterPress(KeyboardEventArgs e, string userName)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                BodyDetails.GroupName = Id;
                BodyDetails.Message = userName + " says: " + ChatInput;
                if (Painting.ContributedUsers.Contains(UserId))
                    await httpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/hub2/sendchatmessage", BodyDetails);
                ChatInput = "";
            }
            return;
        }

        public async Task ContributionRequestDeclined()
        {
            await httpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/hub2/sendcontributionresponse",
                new { UserId = ContributionRequesters.Last().Key, IsAccepted = "false" });
            ContributionRequesters.Remove(ContributionRequesters.Last().Key);
            if (ContributionRequesters.Count == 0)
                DisplayContributionRequestModal = "none";
            StateHasChanged();
        }
        public async Task ContributionRequestAccepted()
        {
            await httpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/hub2/sendcontributionresponse",
                new { UserId = ContributionRequesters.Last().Key, IsAccepted = "true" });
            ContributionRequesters.Remove(ContributionRequesters.Last().Key);
            if (ContributionRequesters.Count == 0)
                DisplayContributionRequestModal = "none";
            StateHasChanged();
        }

        public void OpenContributeModal()
        {
            DisplayContributeModal = "block";
        }
        public void CloseContributeModal()
        {
            DisplayContributeModal = "none";
        }
        public async Task ContributionAccepted(string name)
        {
            if (!Painting.ContributedUsers.Contains(UserId))
            {
                DisplayContributeModal = "none";
                ContributeState = "PENDING";
                IsContributed = true;
                BodyDetails.ConnectionId = Connection.ConnectionId;
                BodyDetails.GroupName = Id;
                BodyDetails.Name = name;
                BodyDetails.UserId = UserId;
                await httpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/hub2/sendcontributionrequest", BodyDetails);
            }
        }
        public void OpenClearModal()
        {
            DisplayClearModal = "block";
        }
        public void CloseClearModal()
        {
            DisplayClearModal = "none";
        }
        public async Task ClearingAccepted()
        {
            Painting.ShapesDetails = new ArrayList();
            Painting.ImageURL = await JsRuntime.InvokeAsync<string>("getNewImage");
            if (Painting.ContributedUsers.Contains(UserId))
                await httpClient.PutAsJsonAsync("https://grouppaintonline-apim.azure-api.net/v1/painting/", Painting);
            return;
        }

        public void OpenShareModal()
        {
            DisplayShareModal = "block";
        }
        public void CloseShareModal()
        {
            DisplayShareModal = "none";
            ReceiverEmailInput = "";
        }
        public async Task SharingAccepted()
        {
            await httpClient.PostAsJsonAsync("https://grouppaintonline-apim.azure-api.net/send/sendinvitationlink", new { ReceiverEmailInput, NavManager.Uri });
            DisplayShareModal = "none";
            ReceiverEmailInput = "";
            return;

        }

        public class WindowDimension
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
