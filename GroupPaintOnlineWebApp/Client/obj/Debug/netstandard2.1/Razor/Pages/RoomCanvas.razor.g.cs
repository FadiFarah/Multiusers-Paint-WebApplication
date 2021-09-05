#pragma checksum "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a4bdfc701d5ec87341be9f4342f69d35a653d2bc"
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
#line 1 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using GroupPaintOnlineWebApp.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\_Imports.razor"
using GroupPaintOnlineWebApp.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
using Blazor.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
using Blazor.Extensions.Canvas;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
using Blazor.Extensions.Canvas.Canvas2D;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/room/{Id}")]
    public partial class RoomCanvas : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "canvas-div");
            __builder.AddMarkupContent(2, "\r    ");
            __builder.OpenComponent<Blazor.Extensions.Canvas.BECanvas>(3);
            __builder.AddAttribute(4, "Width", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int64>(
#nullable restore
#line 10 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
                       (Width)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(5, "Height", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int64>(
#nullable restore
#line 10 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
                                           Height-((Height/4))

#line default
#line hidden
#nullable disable
            ));
            __builder.AddComponentReferenceCapture(6, (__value) => {
#nullable restore
#line 10 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
                                                                       _canvasReference = (Blazor.Extensions.Canvas.BECanvas)__value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseComponent();
            __builder.AddMarkupContent(7, "\r");
            __builder.CloseElement();
            __builder.AddMarkupContent(8, "\r\r");
            __builder.OpenElement(9, "div");
            __builder.AddAttribute(10, "class", "d-flex justify-content-around paint-tool-box");
            __builder.AddAttribute(11, "style", "background-color:white;width:" + (
#nullable restore
#line 13 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
                                                                                                Width+"px"

#line default
#line hidden
#nullable disable
            ) + ";" + " height:" + (
#nullable restore
#line 13 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
                                                                                                                      (Height/4)+"px"

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(12, "\r\r    ");
            __builder.AddMarkupContent(13, "<div>\r        <input id=\"undo\" class=\"paint-tool\" type=\"image\" src=\"http://simpleicon.com/wp-content/uploads/undo-6.png\">\r    </div>\r    ");
            __builder.AddMarkupContent(14, "<div>\r        <input id=\"redo\" class=\"paint-tool\" type=\"image\" src=\"http://simpleicon.com/wp-content/uploads/redo-6.png\">\r    </div>\r    ");
            __builder.AddMarkupContent(15, "<div>\r        <input id=\"clear\" class=\"paint-tool\" type=\"image\" src=\"https://icons-for-free.com/iconfiles/png/512/trash+bin+icon-1320086460670911435.png\">\r    </div>\r    ");
            __builder.AddMarkupContent(16, "<div>\r        <input id=\"eraser\" class=\"paint-tool\" type=\"image\" src=\"https://cdn0.iconfinder.com/data/icons/outline-icons/320/Eraser-512.png\">\r    </div>\r    ");
            __builder.AddMarkupContent(17, "<div class=\"input-color-container\">\r        <input class=\"paint-tool\" id=\"color\" type=\"color\">\r    </div>\r    ");
            __builder.AddMarkupContent(18, "<div>\r        <input class=\"paint-tool styled-slider slider-progress\" id=\"size\" type=\"range\" min=\"1\" max=\"100\" value=\"2\">\r    </div>\r");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 34 "D:\DISK\Multiusers-Paint-WebApplication\GroupPaintOnlineWebApp\Client\Pages\RoomCanvas.razor"
        [Parameter]    public string Id { get; set; }    public int Height { get; set; }    public int Width { get; set; }    public ElementReference ToolBox;    private Canvas2DContext _context;    protected BECanvasComponent _canvasReference;    protected override async Task OnInitializedAsync()    {        var dimension = await JsRuntime.InvokeAsync<WindowDimension>("getWindowDimensions");        Height = dimension.Height;        Width = dimension.Width;    }    protected override async Task OnAfterRenderAsync(bool firstRender)    {        this._context = await this._canvasReference.CreateCanvas2DAsync();        await this._context.SetFillStyleAsync("red");        await this._context.FillRectAsync(10, 100, 100, 100);        await this._context.SetFontAsync("38px Calibri");        await this._context.StrokeTextAsync("Hello Blazor!!!", 5, 100);    }    public class WindowDimension    {        public int Width { get; set; }        public int Height { get; set; }    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JsRuntime { get; set; }
    }
}
#pragma warning restore 1591
