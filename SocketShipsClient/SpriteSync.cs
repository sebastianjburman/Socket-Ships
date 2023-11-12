using System.Net.Sockets;
using System.Text;
using System;

namespace SocketShipsClient;

public static class SpriteSync
{
    private static TcpClient client;
    private static NetworkStream stream;

    // Set these at creation of the game through --flags
    // Default to these if no input
    public static string IpAddress = "127.0.0.1";
    public static int Port = 8080;

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

    public static void CloseAndDispose()
    {
        // Close the connection
        CloseConnection();
    }
}
