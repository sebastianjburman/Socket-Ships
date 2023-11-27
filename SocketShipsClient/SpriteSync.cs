using System.Net.Sockets;
using System.Text;
using System;
using SocketShipsClient.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocketShipsClient
{
    public static class SpriteSync
    {
        private static Socket clientSocket;

        // Set these at the creation of the game through --flags
        public static string IpAddress;
        public static int Port;

        public static void SetIPAndPort(string ip, int port)
        {
            IpAddress = ip;
            Port = port;
        }

        public static void InitializeConnection()
        {
            // Create the Socket and connect to the server
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(IpAddress, Port);
            Console.WriteLine("Connected to server");
        }

        private static void CloseConnection()
        {
            // Close the connection
            if (clientSocket != null && clientSocket.Connected)
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                Console.WriteLine("Connection closed");
            }
        }

        public static void SendToServer(string spriteData)
        {
            try
            {
                // Send a message to the server
                byte[] buffer = Encoding.ASCII.GetBytes(spriteData);
                clientSocket.Send(buffer);
                Console.WriteLine($"Sent message to server: {spriteData}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }

        public static async Task<SpriteSyncModel> ReceiveFromServer()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
            
            string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Receiving Message {clientMessage}");
            SpriteSyncModel data = JsonSerializer.Deserialize<SpriteSyncModel>(clientMessage);
            return data;
        }

        public static void CloseAndDispose()
        {
            // Close the connection
            CloseConnection();
        }
    }
}
