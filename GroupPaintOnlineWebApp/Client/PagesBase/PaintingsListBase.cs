using GroupPaintOnlineWebApp.Shared;
using GroupPaintOnlineWebApp.Shared.RESTServices.ServicesInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Client.PagesBase
{
    public class PaintingsListBase : ComponentBase
    {
        [Inject]
        public IPaintingService PaintingService { get; set; }
        [Inject]
        public NavigationManager NavManager { get; set; }
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public List<Painting> Paintings { get; set; }
        public string URL { get; set; }
        public string DisplayImage { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                Paintings = (await PaintingService.GetPaintings()).ToList();
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
        public async Task DeletePaintingClick(string Id)
        {
            var response = await PaintingService.DeletePainting(Id);
            if (response.IsSuccessStatusCode)
            {
                Paintings = (await PaintingService.GetPaintings()).ToList();
                StateHasChanged();
            }
        }
    }
}
