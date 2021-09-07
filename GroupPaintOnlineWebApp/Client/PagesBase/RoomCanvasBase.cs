using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using GroupPaintOnlineWebApp.Shared;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        //Parameters
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string Password { get; set; }


        public BECanvasComponent _canvasReference { get; set; }

        public int Height { get; set; }
        public int Width { get; set; }
        


        protected override async Task OnInitializedAsync()
        {
            var dimension = await JsRuntime.InvokeAsync<WindowDimension>("getWindowDimensions");
            await JsRuntime.InvokeVoidAsync("onInitialized");
            Height = dimension.Height;
            Width = dimension.Width;
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
            }

        }

        public class WindowDimension
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
