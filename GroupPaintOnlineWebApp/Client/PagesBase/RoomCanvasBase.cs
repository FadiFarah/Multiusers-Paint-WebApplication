﻿using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using GroupPaintOnlineWebApp.Shared;

using GroupPaintOnlineWebApp.Shared.Services;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
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

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class RoomCanvasBase : ComponentBase
    {
        //Injections
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        [Inject]
        public IRoomService RoomService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }


        //RouteUri Parameters
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string Password { get; set; }

        //Hub Connection Related Properties
        private HubConnection Connection { get; set; }
        public string URL { get; set; }
        public string ConnectionStatus { get; set; }

        //Canvas Related Properties
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsPainting { get; set; }
        public string ImageURL { get; set; }



        protected override async Task OnInitializedAsync()
        {
            var response = await RoomService.GetRoom(Id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                NavManager.NavigateTo("/roomslist");
            }
            else
            {
                var room = response.Content.ReadFromJsonAsync<Room>();
                if (room.Result.Password != Password)
                {
                    NavManager.NavigateTo("/roomslist");
                }
                else
                {
                    var dimension = await JsRuntime.InvokeAsync<WindowDimension>("getWindowDimensions");
                    await JsRuntime.InvokeVoidAsync("onInitialized");
                    Height = dimension.Height;
                    Width = dimension.Width;
                    await ConnectToServer();
                    
                }
            }
        }

        private async Task ConnectToServer()
        {
            URL = "https://localhost:44301";
            Connection = new HubConnectionBuilder().WithUrl(URL+ "/roomCanvasHub").Build();
            await Connection.StartAsync();
            ConnectionStatus = "Connected!";
            Console.WriteLine(ConnectionStatus);

            Connection.Closed += async (s) =>
            {
                ConnectionStatus = "Disconnected";
                Console.WriteLine(ConnectionStatus);
                await Connection.StartAsync();
            };

            Connection.On<string>("UserConnected", async (connectionId) =>
            {
                Console.WriteLine("User Connected");
                await Connection.InvokeAsync("AddToGroup", Id, connectionId);
            });
            Connection.On<string>("UserDisconnected", async (connectionId) =>
            {
                Console.WriteLine("User Disconnected");
                await Connection.InvokeAsync("RemoveFromGroup", Id, connectionId);
            });
            Connection.On<string,string>("ReceiveContext", async (imageURL, connectionId) =>
            {
                if(connectionId!=Connection.ConnectionId)
                {
                    await JsRuntime.InvokeAsync<string>("drawImage", imageURL);
                }
            });
        }

        public void CanvasOnMouseDown()
        {
            IsPainting = true;
        }

        public async Task CanvasOnMouseUp()
        {
            IsPainting = false;
            ImageURL = await JsRuntime.InvokeAsync<string>("getNewImage");
            await Connection.InvokeAsync("SendContext", ImageURL, Id);
        }
        public void CanvasOnMouseOut()
        {
            IsPainting = false;
        }

        public class WindowDimension
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
