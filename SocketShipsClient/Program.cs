using System.Net;
using SocketShipsClient;
using CommandLine;

class Program
{
    class Options
    {
        [Option('i', "ip", Required = true, HelpText = "IP address")]
        public string IpAddress { get; set; }

        [Option('p', "port", Required = true, HelpText = "Port")]
        public int Port { get; set; }
    }

    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(options =>
            {
                SpriteSync.SetIPAndPort(options.IpAddress,options.Port);
                SpriteSync.InitializeConnection();
                using var game = new SocketShipsClient.SocketShips();
                game.Run();
                SpriteSync.CloseAndDispose();
            });
    }
}