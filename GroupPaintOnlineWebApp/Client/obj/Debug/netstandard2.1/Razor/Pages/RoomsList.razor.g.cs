#pragma checksum "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "aec794650a2ee4b31d4884eb12afc84f78f2cd56"
// <auto-generated/>
#pragma warning disable 1591
namespace GroupPaintOnlineWebApp.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using GroupPaintOnlineWebApp.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using GroupPaintOnlineWebApp.Client.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/roomslist")]
    public partial class RoomsList : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<div class=\"d-flex justify-content-center mb-3 align-items-stretch\" style=\"width:100%\">\r    <div class=\"p-2\"><h1>ROOMS LIST</h1></div>\r</div>\r\r\r");
            __builder.AddMarkupContent(1, "<div><a href=\"/createroom\">Create New</a></div>\r\r\r\r\n\r\n\r\n\r");
            __builder.AddMarkupContent(2, @"<div class=""d-flex justify-content-center mb-3 align-items-center text-center"" style=""width:100%"">    <table cellspacing=""0"" cellpadding=""0"" class=""table"">        <thead>            <tr>                <th>Room Name</th>                <th>Current Users</th>                <th>Is Public</th>                <th>Password</th>            </tr>        </thead>        <tbody>            <tr>                <td>Painting a Dog</td>                <td>3</td>                <td>true</td>                <td><input value type=""password""><span class=""auth-container""><a class=""auth-btn"" href=""/room/{id}/"">Join</a></span></td>            </tr>            <tr>                <td>Painting a Cat</td>                <td>2</td>                <td>true</td>                <td><input value type=""password""><span class=""auth-container""><a class=""auth-btn"" href=""/room/{id}/"">Join</a></span></td>            </tr>        </tbody>    </table></div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
