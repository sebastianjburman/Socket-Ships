using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SocketShipsClient.Sprites;

public class HeroBullet:Sprite
{
    private int speed = 500;
    public HeroBullet(string spriteTextureFileName, Vector2 spritePosition) : base(spriteTextureFileName, spritePosition)
    {
    }

    public override void Update(GameTime gameTime, GraphicsDevice gd)
    {
        _SpritePosition.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_SpriteTexture, _SpritePosition, Color.White);
    }
}