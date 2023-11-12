using System;
using Microsoft.Xna.Framework;
namespace SocketShipsClient.Models;

public class SpriteSyncModel
{
    public Guid GUID { get; set; }
    public string Type { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public SpriteSyncModel(Guid guid, string type, float x, float y)
    {
        GUID = guid;
        Type = type;
        X = x;
        Y = y;
    }
}