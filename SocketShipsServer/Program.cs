using System.Net;
using System.Net.Sockets;

public class SocketShipServer
{
    private static List<TcpClient> clients = new List<TcpClient>();
    static async Task Main()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8080;
        TcpListener server = new TcpListener(ipAddress, port);
        server.Start();
        Console.WriteLine($"Server listening on port {port}");
        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            //Lock list of clients to this thread when adding
            lock (clients)
            {
                clients.Add(client);
            }
            AddClient(client);
        }
    }

    public static async Task AddClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        try
        {
            //Read in buffer size
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                //Broadcast message to all clients besides who send this message
                await BroadcastMessage(buffer, bytesRead, client);
            }
        }
        catch
        {
            // Handle exceptions or disconnects
        }
        //Lock list of clients to this thread when removing
        lock (clients)
        {
            clients.Remove(client);
        }
        client.Close();
    }

    public static async Task BroadcastMessage(byte[] buffer, int bytesRead, TcpClient senderClient)
    {
        List<Task> sendTasks = new List<Task>();
        lock (clients)
        {
            foreach (TcpClient client in clients)
            {
                //Make sure to not send to who originally sent the message
                if (client != senderClient)
                {
                    sendTasks.Add(client.GetStream().WriteAsync(buffer, 0, bytesRead));
                }
            }
        }
        await Task.WhenAll(sendTasks);
    }
}