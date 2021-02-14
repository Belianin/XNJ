using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XNJ.Network;

namespace XNJ.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new XnjClient();
            client.OnMessage += (s, e) =>
            {
                if (e is PlayerServerMessage message)
                {
                    Console.WriteLine(message.Player.X + ":" + message.Player.Y);
                }
            };

            await client.ListenAsync();
        }
    }
}