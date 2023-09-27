using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Easy14_Programming_Language
{
    public static class NetworkWebSocket
    {
        public static ClientWebSocket clientWebSocket;

        public static async Task Connect(string url)
        {
            clientWebSocket = new ClientWebSocket();
            await clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);

            // Start a new thread to handle receiving messages
            await Task.Run(async () =>
            {
                while (true)
                {
                    Console.Write(clientWebSocket);
                    byte[] buffer = new byte[1024];
                    WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    string receivedMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"\nReceived: {receivedMessage}");
                }
            });
        }

        public static async Task CloseConnection()
        {
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by user", CancellationToken.None);
        }

        public static async Task SendMessage(string message)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            await clientWebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            Console.WriteLine($"Sent: {message}");
        }
    }

    public static class WebSocketActions
    {
        public static async Task Interpret(string action, string message = "")
        {
            switch (action.ToLower())
            {
                case "connect":
                    await NetworkWebSocket.Connect(message);
                    break;

                case "send":
                    await NetworkWebSocket.SendMessage(message);
                    break;

                case "close":
                    await NetworkWebSocket.CloseConnection();
                    break;

                default:
                    Console.WriteLine("Invalid action.");
                    break;
            }
        }
    }
}
