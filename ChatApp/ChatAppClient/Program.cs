using System;
using System.Net;
using System.Text;
using System.Threading;
using MetaMitStandard;
using MetaMitStandard.Utils;

namespace ChatAppClient
{
    class Program
    {
        // Uses MetaMitStandard v0.1.0
        static void Main(string[] args)
        {
            MetaMitClient client = new MetaMitClient();

            IPAddress ip = MetaMitStandard.Utils.ConsoleUtils.ConsoleQuestions.AskIP();
            ushort port = MetaMitStandard.Utils.ConsoleUtils.ConsoleQuestions.AskPort();
            string username = MetaMitStandard.Utils.ConsoleUtils.ConsoleQuestions.AskUsername();

            IPEndPoint ep = MetaMitStandard.Utils.NetworkUtils.GetEndPoint(ip, port);

            client.Connected += (object sender, MetaMitStandard.Client.ConnectedEventArgs e) =>
            {
                Console.WriteLine("Connected to server!");
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

            while (true)
            {
                string message = Console.ReadLine();
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    if (message.ToLower() == "exit")
                        break;
                    client.Send(Encoding.ASCII.GetBytes(username + ": " + message));
                }
            }
        }
    }
}
