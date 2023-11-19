using System;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SocketShipsClient.Sprites;

public class Background:Sprite
{
    public Background(string spriteTextureFileName, Vector2 spritePosition, Guid spriteId) : base(spriteTextureFileName, spritePosition, spriteId)
    {
    }

    public override void Update(GameTime gameTime, GraphicsDevice gd, ConcurrentDictionary<Guid, ISprite> sprites)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_SpriteTexture, _SpritePosition, new Rectangle((int)_SpritePosition.X,(int)_SpritePosition.Y,_SpriteTexture.Width,_SpriteTexture.Height),Color.White);
    }
}