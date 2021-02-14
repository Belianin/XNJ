using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XNJ.Network
{
    public class XnjClient : IDisposable
    {
        private readonly ClientWebSocket webSocket;

        public event EventHandler<XnjMessage> OnMessage; 
        
        public XnjClient()
        {
            webSocket = new ClientWebSocket();
        }

        public async Task RunAsync()
        {
            await webSocket.ConnectAsync(new Uri("ws://localhost:5001"), CancellationToken.None);
            
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var stringValue = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                var deserialized = JsonConvert.DeserializeObject<XnjMessage>(stringValue, new MessageJsonConverter());
                OnMessage?.Invoke(this, deserialized);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        
        public void Dispose()
        {
            webSocket?.Dispose();
        }
    }
}