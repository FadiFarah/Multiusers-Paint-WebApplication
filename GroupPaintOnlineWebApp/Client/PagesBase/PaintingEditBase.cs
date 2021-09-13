using GroupPaintOnlineWebApp.Shared;
using GroupPaintOnlineWebApp.Shared.RESTServices.ServicesInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class PaintingEditBase : ComponentBase
    {
        //Injections
        [Inject]
        public IJSRuntime JsRuntime { get; set; }
        [Inject]
        public IPaintingService PaintingService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }



        //RouteUri Parameters
        [Parameter]
        public string Id { get; set; }


        //Canvas Related Properties
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsPainting { get; set; }
        public string ImageURL { get; set; }
        public string DisplayOtherBox { get; set; }
        public Painting Painting;

        protected override async Task OnInitializedAsync()
        {
            var response = await PaintingService.GetPainting(Id);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                NavManager.NavigateTo("/paintingsList");
            }
            else
            {
                Painting = response.Content.ReadFromJsonAsync<Painting>().Result;
                DisplayOtherBox = "none";
                var dimension = await JsRuntime.InvokeAsync<WindowDimension>("getWindowDimensions");
                await JsRuntime.InvokeVoidAsync("onInitialized");
                Height = dimension.Height;
                Width = dimension.Width;
                await JsRuntime.InvokeVoidAsync("drawImage", Painting.ImageURL);
                ImageURL = Painting.ImageURL;
                
            }
        }

        public void CanvasOnMouseDown()
        {
            IsPainting = true;
        }

        public async Task CanvasOnMouseUp()
        {
            IsPainting = false;
            ImageURL = await JsRuntime.InvokeAsync<string>("getNewImage");
            Painting.ImageURL = ImageURL;
        }
        public void CanvasOnMouseOut()
        {
            IsPainting = false;
        }

        public void OtherButtonClick()
        {
            if (DisplayOtherBox == "none")
                DisplayOtherBox = "block";
            else
                DisplayOtherBox = "none";
        }

        public async Task SaveButtonClick()
        {
            var response=await PaintingService.PutPainting(Id, Painting);
            if (response.IsSuccessStatusCode)
            {
                NavManager.NavigateTo("paintingsList");
            }
        }

        public class WindowDimension
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
}
