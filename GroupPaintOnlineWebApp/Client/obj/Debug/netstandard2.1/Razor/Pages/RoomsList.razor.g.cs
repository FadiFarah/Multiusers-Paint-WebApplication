#pragma checksum "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0abc22966823c136500b6dde9116d448fea99383"
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
#nullable restore
#line 2 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
           [Authorize]

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/roomslist")]
    public partial class RoomsList : GroupPaintOnlineWebApp.Client.PagesBase.RoomsListBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<div class=\"d-flex justify-content-center mb-3 align-items-stretch\" style=\"width:100%\">\r\n    <div class=\"p-2\"><h1>ROOMS LIST</h1></div>\r\n</div>\r\n\r\n\r\n");
            __builder.AddMarkupContent(1, "<div><a href=\"/createroom\">Create New</a></div>\r\n\r\n");
#nullable restore
#line 13 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
 if (Rooms == null)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(2, "    ");
            __builder.AddMarkupContent(3, "<h1>Loading...</h1>\r\n");
#nullable restore
#line 16 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
}
else
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(4, "    ");
            __builder.OpenElement(5, "div");
            __builder.AddAttribute(6, "class", "d-flex justify-content-center mb-3 align-items-center text-center");
            __builder.AddAttribute(7, "style", "width:100%");
            __builder.AddMarkupContent(8, "\r\n        ");
            __builder.OpenElement(9, "table");
            __builder.AddAttribute(10, "cellspacing", "0");
            __builder.AddAttribute(11, "cellpadding", "0");
            __builder.AddAttribute(12, "class", "table");
            __builder.AddMarkupContent(13, "\r\n            ");
            __builder.AddMarkupContent(14, "<thead>\r\n                <tr>\r\n                    <th>Room Name</th>\r\n                    <th>Current Users</th>\r\n                    <th>Is Public</th>\r\n                    <th>Password</th>\r\n                </tr>\r\n            </thead>\r\n            ");
            __builder.OpenElement(15, "tbody");
            __builder.AddMarkupContent(16, "\r\n");
#nullable restore
#line 30 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                 for (int i = 0; i < Rooms.Length; ++i)
                {
                    int local_i = i;

#line default
#line hidden
#nullable disable
            __builder.AddContent(17, "                    ");
            __builder.OpenElement(18, "tr");
            __builder.AddMarkupContent(19, "\r\n                        ");
            __builder.OpenElement(20, "td");
            __builder.AddContent(21, 
#nullable restore
#line 34 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                             Rooms[local_i].RoomName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n                        ");
            __builder.OpenElement(23, "td");
            __builder.AddContent(24, 
#nullable restore
#line 35 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                             Rooms[local_i].CurrentUsers

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(25, "\r\n                        ");
            __builder.OpenElement(26, "td");
            __builder.AddContent(27, 
#nullable restore
#line 36 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                             Rooms[local_i].IsPublic

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(28, "\r\n                        ");
            __builder.OpenElement(29, "td");
            __builder.AddMarkupContent(30, "\r\n                            ");
            __builder.OpenComponent<Microsoft.AspNetCore.Components.Forms.EditForm>(31);
            __builder.AddAttribute(32, "Model", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Object>(
#nullable restore
#line 38 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                                              RoomsList[local_i]

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(33, "OnSubmit", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<Microsoft.AspNetCore.Components.Forms.EditContext>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Forms.EditContext>(this, 
#nullable restore
#line 38 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                                                                            FormSubmitted

#line default
#line hidden
#nullable disable
            )));
            __builder.AddAttribute(34, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment<Microsoft.AspNetCore.Components.Forms.EditContext>)((context) => (__builder2) => {
                __builder2.AddMarkupContent(35, "\r\n");
#nullable restore
#line 39 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                                 if (Rooms[local_i].IsPublic)
                                {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(36, "                                    ");
                __builder2.OpenElement(37, "input");
                __builder2.AddAttribute(38, "disabled", true);
                __builder2.AddAttribute(39, "id", "Password");
                __builder2.AddAttribute(40, "type", "password");
                __builder2.AddAttribute(41, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 41 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                                                         RoomsList[local_i].Password

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(42, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => RoomsList[local_i].Password = __value, RoomsList[local_i].Password));
                __builder2.SetUpdatesAttributeName("value");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(43, "\r\n");
#nullable restore
#line 42 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                                }
                                else
                                {

#line default
#line hidden
#nullable disable
                __builder2.AddContent(44, "                                    ");
                __builder2.OpenElement(45, "input");
                __builder2.AddAttribute(46, "id", "Password");
                __builder2.AddAttribute(47, "type", "password");
                __builder2.AddAttribute(48, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 45 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                                                         RoomsList[local_i].Password

#line default
#line hidden
#nullable disable
                ));
                __builder2.AddAttribute(49, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => RoomsList[local_i].Password = __value, RoomsList[local_i].Password));
                __builder2.SetUpdatesAttributeName("value");
                __builder2.CloseElement();
                __builder2.AddMarkupContent(50, "\r\n");
#nullable restore
#line 46 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                                }

#line default
#line hidden
#nullable disable
                __builder2.AddContent(51, "                                ");
                __builder2.AddMarkupContent(52, "<span class=\"auth-container\">\r\n                                    <input type=\"submit\" value=\"Join\" class=\"auth-btn\">\r\n                                </span>\r\n                            ");
            }
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(53, "\r\n                        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(54, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(55, "\r\n");
#nullable restore
#line 53 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(56, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(57, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(58, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(59, "\r\n");
#nullable restore
#line 57 "C:\Users\fadif\source\repos\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomsList.razor"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
