using GroupPaintOnlineWebApp.Shared;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Rooms = (await RoomService.GetRooms()).ToArray();
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
