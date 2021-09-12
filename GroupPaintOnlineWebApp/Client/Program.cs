using GroupPaintOnlineWebApp.Shared.RESTServices;
using GroupPaintOnlineWebApp.Shared.RESTServices.ServicesInterfaces;
using GroupPaintOnlineWebApp.Shared.Services;
using GroupPaintOnlineWebApp.Shared.Services.ServicesInterfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GroupPaintOnlineWebApp.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddHttpClient("GroupPaintOnlineWebApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GroupPaintOnlineWebApp.ServerAPI"));

            builder.Services.AddApiAuthorization();
            builder.Services.AddHttpClient<IRoomService, RoomService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44301/");
            });
            builder.Services.AddHttpClient<IPaintingService, PaintingService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44301/");
            });
            await builder.Build().RunAsync();
        }
    }
}
