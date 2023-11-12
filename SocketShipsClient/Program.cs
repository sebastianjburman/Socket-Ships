
using SocketShipsClient;

using var game = new SocketShipsClient.SocketShips();
SpriteSync.InitializeConnection();
game.Run();
SpriteSync.CloseAndDispose();
