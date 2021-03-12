using System;
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
                Console.WriteLine(e.Number);
            };

            await client.ListenAsync();
        }
    }
}