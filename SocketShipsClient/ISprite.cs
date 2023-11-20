using System;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SocketShipsClient;

public interface ISprite
{
    void LoadContent(ContentManager cm);
    void Update(GameTime gameTime, GraphicsDevice gd, ConcurrentDictionary<Guid,ISprite> sprites);
    void Draw(SpriteBatch spriteBatch);
    Vector2 GetPosition();
    void SetPosition(Vector2 position);
    Guid GetGuid();
    Rectangle GetSpritRectangle();
    void SyncUp(bool deleteSprite);
}