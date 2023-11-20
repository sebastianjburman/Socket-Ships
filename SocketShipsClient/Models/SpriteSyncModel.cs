using System;
using Microsoft.Xna.Framework;
namespace SocketShipsClient.Models;

public class SpriteSyncModel
{
    public Guid GUID { get; set; }
    public string Type { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public bool RemoveSprite { get; set; }
    public SpriteSyncModel(Guid guid, string type, float x, float y,bool removeSprite)
    {
        GUID = guid;
        Type = type;
        X = x;
        Y = y;
        RemoveSprite = removeSprite;
    }
}