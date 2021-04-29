using System;
using System.Net;
using System.Text;
using System.Threading;
using MetaMitStandard;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            MetaMitClient client = new MetaMitClient();

            IPAddress ip = IPAddress.Parse("192.168.1.92");
            ushort port = 25562;

            IPEndPoint ep = MetaMitStandard.Utils.NetworkUtils.GetEndPoint(ip, port);

            client.Connected += (object sender, MetaMitStandard.Client.ConnectedEventArgs e) =>
            {
                Console.WriteLine("Connected to server, Sending data!");
                for (int i = 0; i < 1; i++)
                {
                    byte[] data = new byte[] { 0x08, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x08, 0x00, 0x00, 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
                    client.SendRaw(data);
                }
            };
            client.DataReceived += (object sender, MetaMitStandard.Client.DataReceivedEventArgs e) =>
            {
                Console.WriteLine(Encoding.ASCII.GetString(e.data));
            };

            client.Connect(ep);

            Thread eventPollingThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    client.PollEvents();
                    Thread.Sleep(5);
                }
            }));
            eventPollingThread.Start();

            Console.ReadLine();
        }
    }
}
