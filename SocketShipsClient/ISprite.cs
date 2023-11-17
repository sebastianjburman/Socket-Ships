using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SocketShipsClient;

public interface ISprite
{
    void LoadContent(ContentManager cm);
    void Update(GameTime gameTime, GraphicsDevice gd);
    void Draw(SpriteBatch spriteBatch);
    Vector2 GetPosition();
    void SetPosition(Vector2 position);
    Guid GetGuid();
}