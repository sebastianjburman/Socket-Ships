using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SocketShipsClient.Sprites;

public class SpaceShip:AnimatedSprite
{
    public SpaceShip(string spriteTextureFileName, Vector2 spritePosition, double frameDuration, int frameCount) : base(spriteTextureFileName, spritePosition, frameDuration, frameCount)
    {
    }

    public override void Update(GameTime gameTime, GraphicsDevice gd)
    {
        AnimateSprite(gameTime,gd);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRectangle = new Rectangle(_CurrentFrame * _FrameWidth, 0, _FrameWidth, _FrameHeight);
        spriteBatch.Draw((_SpriteTexture), _SpritePosition, sourceRectangle, Color.White,0,new Vector2((_SpriteTexture.Width/this._FrameCount)/2,_SpriteTexture.Height/2),1,SpriteEffects.None,0);
    }
}