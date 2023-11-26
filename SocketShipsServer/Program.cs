using System.Net;
using System.Net.Sockets;
using CommandLine;
using System.Text;


public class SocketShipServer
{
    private static List<TcpClient> clients = new List<TcpClient>();
    class Options
    {
        [Option('i', "ip", Required = true, HelpText = "IP address")]
        public string? IpAddress { get; set; }
    
        [Option('p', "port", Required = true, HelpText = "Port")]
        public int Port { get; set; }
    }
    public static async Task Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8080;

        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options =>
            {
                ipAddress = IPAddress.Parse(options.IpAddress);
                port = options.Port;
            });
        TcpListener server = new TcpListener(ipAddress, port);
        server.Start();
        Console.WriteLine($"Server listening on port {port}");
        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            // Lock list of clients when adding
            lock (clients)
            { 
                clients.Add(client);
            }
            //In this method we call await stream.ReadAsync so when waiting for this we break out of this method and continue the
            //while loop. If I await this then we wait until this method to finish
            ProcessMessagesForClient(client);
        }
    }

    public static async Task ProcessMessagesForClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;
        try
        {
            // Read in buffer size
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received {clientMessage} from {((IPEndPoint)client.Client.RemoteEndPoint).Address}. Broadcasting message.");
                // Broadcast message to all clients besides who sent this message
                await BroadcastMessage(buffer, bytesRead, client);
            }
        }
        catch
        {
        }
        // Lock list of clients to this thread when removing
        lock (clients)
        {
            clients.Remove(client);
        }
        Console.WriteLine("Closing client connection");
        client.Close();
    }

    public static async Task BroadcastMessage(byte[] buffer, int bytesRead, TcpClient senderClient)
    {
        List<Task> sendTasks = new List<Task>();
        lock (clients)
        {
            foreach (TcpClient client in clients)
            {
                // Make sure not to send to who originally sent the message
                if (client != senderClient)
                {
                    sendTasks.Add(client.GetStream().WriteAsync(buffer, 0, bytesRead));
                }
            }
        }
        await Task.WhenAll(sendTasks);
    }

    
}