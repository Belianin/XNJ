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

        public event EventHandler<XnjServerMessage> OnMessage; 
        
        public XnjClient()
        {
            webSocket = new ClientWebSocket();
        }

        public async Task ListenAsync()
        {
            await webSocket.ConnectAsync(new Uri("ws://localhost:5001"), CancellationToken.None);
            
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                var stringValue = Encoding.UTF8.GetString(new ArraySegment<byte>(buffer, 0, result.Count));
                var deserialized = JsonConvert.DeserializeObject<XnjServerMessage>(stringValue, new ServerMessageJsonConverter());
                OnMessage?.Invoke(this, deserialized);
                
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }

        public Task SendAsync(XnjServerMessage message)
        {
            if (webSocket.State != WebSocketState.Open)
                return Task.CompletedTask;
            
            var stringValue = JsonConvert.SerializeObject(message);
            
            return webSocket.SendAsync(Encoding.UTF8.GetBytes(stringValue), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        
        public void Dispose()
        {
            webSocket?.Dispose();
        }
    }
}