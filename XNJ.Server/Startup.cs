using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using XNJ.Network;

namespace XNJ
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using (WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync())
                    {
                        await Echo(context, webSocket);
                    }
                }
                else
                {
                    await next();
                }
            });
        }
        
        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var i = 0;
            var buffer = new byte[1024 * 4];
            while (!webSocket.CloseStatus.HasValue)
            {
                i++;
                Console.WriteLine(i.ToString());
                var player = new PlayerServerMessage
                {
                    Player = new Player
                    {
                        X = 0,
                        Y = i
                    }
                };
                var count = JsonConvert.SerializeObject(player);
                await webSocket.SendAsync(Encoding.UTF8.GetBytes(count), WebSocketMessageType.Text, true, CancellationToken.None);

                await Task.Delay(1000);
                //result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(webSocket.CloseStatus.Value, webSocket.CloseStatusDescription, CancellationToken.None);
        }
    }
}
