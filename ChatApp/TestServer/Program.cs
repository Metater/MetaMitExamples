using System;
using System.Text;
using System.Threading;
using MetaMitStandard;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int listeningPort = 25562;
            int maxConnectionBacklog = 100;
            MetaMitServer server = new MetaMitServer(listeningPort, maxConnectionBacklog);
            server.ClientConnected += (object sender, MetaMitStandard.Server.ClientConnectedEventArgs e) =>
            {
                Console.WriteLine("Client Connected: \n" + "\tGuid: " + e.guid + "\n" + "\tEndpoint: " + e.ep);
            };
            server.DataReceived += (object sender, MetaMitStandard.Server.DataReceivedEventArgs e) =>
            {
                Console.WriteLine($"Received {e.data.Length} bytes: ");
                foreach(byte b in e.data)
                {
                    Console.WriteLine("\t" + b);
                }
            };

            server.Start();
            Console.WriteLine("Listening on: " + server.ep);

            while (!Console.KeyAvailable)
            {
                server.PollEvents();
                Thread.Sleep(5);
            }
        }
    }
}
