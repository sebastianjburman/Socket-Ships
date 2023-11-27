using System;
using System.Net;
using System.Net.Sockets;
using CommandLine;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

public class SocketShipServer
{
    private static List<Socket> clients = new List<Socket>();
    
    class Options
    {
        [Option('i', "ip", Required = true, HelpText = "IP address")]
        public string? IpAddress { get; set; }

        [Option('p', "port", Required = true, HelpText = "Port")]
        public int Port { get; set; }
    }

    public static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args)
            .WithParsedAsync(options => StartServerAsync(options.IpAddress, options.Port));
    }

    private static async Task StartServerAsync(string ipAddress, int port)
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));
        serverSocket.Listen();

        Console.WriteLine($"Server listening on port {port}");

        while (true)
        {
            Socket clientSocket = await serverSocket.AcceptAsync();
            clients.Add(clientSocket);
            
            //Execution of this current method continues before ProcessMessagesForClient finishes
            //since this method is async and we don't use await.
            ProcessMessagesForClient(clientSocket);
        }
    }

    public static async Task ProcessMessagesForClient(Socket clientSocket)
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            // Read in buffer size
            while ((bytesRead = await clientSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None)) > 0)
            {
                string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received {clientMessage} from {((IPEndPoint)clientSocket.RemoteEndPoint).Address}. Broadcasting message.");

                // Broadcast message to all clients besides who sent this message
                await BroadcastMessage(buffer, bytesRead, clientSocket);
            }
        }
        catch
        {
            Console.WriteLine("Error receiving message");
        }

        clients.Remove(clientSocket);
        Console.WriteLine("Closing client connection");
        clientSocket.Close();
    }

    public static async Task BroadcastMessage(byte[] buffer, int bytesRead, Socket senderSocket)
    {
        List<Task> sendTasks = new List<Task>();

        foreach (Socket clientSocket in clients)
        {
            // Make sure not to send to who originally sent the message
            if (clientSocket != senderSocket)
            {
                sendTasks.Add(clientSocket.SendAsync(new ArraySegment<byte>(buffer, 0, bytesRead), SocketFlags.None));
            }
        }

        await Task.WhenAll(sendTasks);
    }
}
