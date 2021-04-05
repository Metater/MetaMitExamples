using System;
using System.Text;
using System.Threading;
using MetaMitStandard;
using MetaMitStandard.Server;

namespace ChatAppServer
{
    class Program
    {
        // Uses MetaMitStandard v0.1.0
        static void Main(string[] args)
        {
            int listeningPort = 25562;
            int maxConnectionBacklog = 100;
            MetaMitServer server = new MetaMitServer(listeningPort, maxConnectionBacklog);

            server.ClientConnected += (object sender, MetaMitStandard.Server.ClientConnectedEventArgs e) =>
            {
                Console.WriteLine("Client Connected: \n" + "\tGuid: " + e.guid + "\n" + "\tEndpoint: " + e.ep);
            };
            server.DataReceived += (object sender, MetaMitStandard.Server.DataReceivedEventArgs e) =>
            {
                server.BroadcastToBut(e.clientConnection, e.data);
                Console.WriteLine("Client Sent Data: \n" + "\tGuid: " + e.clientConnection.guid + "\n" + "\tData: " + Encoding.ASCII.GetString(e.data));
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
