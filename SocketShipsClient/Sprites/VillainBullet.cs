using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Concurrent;

namespace SocketShipsClient.Sprites;

public class VillainBullet : Sprite
{
    private int speed = 1000;
    public VillainBullet(string spriteTextureFileName, Vector2 spritePosition,Guid spriteId) : base(spriteTextureFileName, spritePosition,spriteId)
    {
    }

    public override void Update(GameTime gameTime, GraphicsDevice gd, ConcurrentDictionary<Guid,ISprite> sprites)
    {
        _SpritePosition.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_SpriteTexture, _SpritePosition, Color.White);
    }
}