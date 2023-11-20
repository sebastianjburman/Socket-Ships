using System.Net.Sockets;
using System.Text;
using System;
using System.Net;
using SocketShipsClient.Models;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocketShipsClient;

public static class SpriteSync
{
    private static TcpClient client;
    private static NetworkStream stream;

    // Set these at creation of the game through --flags
    public static string IpAddress;
    public static int Port;

    public static void SetIPAndPort(string ip, int port)
    {
        IpAddress= ip;
        Port = port;
    }
    public static void InitializeConnection()
    {
        // Create the TcpClient and connect to the server
        client = new TcpClient(IpAddress, Port);
        Console.WriteLine("Connected to server");

        // Get the network stream
        stream = client.GetStream();
    }

    private static void CloseConnection()
    {
        // Close the connection
        if (client != null)
        {
            client.Close();
            Console.WriteLine("Connection closed");
        }
    }

    public static void SendToServer(string spriteData)
    {
        try
        {
            // Send a message to the server
            byte[] buffer = Encoding.ASCII.GetBytes(spriteData);
            stream.Write(buffer, 0, buffer.Length);
            Console.WriteLine($"Sent message to server: {spriteData}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message: {ex.Message}");
        }
    }
    //This is asynchronous so this won't hold up the game
    public static async Task<SpriteSyncModel> ReceiveFromServer()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        SpriteSyncModel data = JsonSerializer.Deserialize<SpriteSyncModel>(clientMessage);
        return data;
    }

    public static void CloseAndDispose()
    {
        // Close the connection
        CloseConnection();
    }
}
