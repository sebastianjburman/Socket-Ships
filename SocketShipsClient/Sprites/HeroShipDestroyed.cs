using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SocketShipsClient.Sprites;

public class HeroShipDestroyed : AnimatedSprite
{
    private float ShipHitAnimationDelay;
    private float ShipHitElapsed;

    public HeroShipDestroyed(Vector2 spritePosition, double frameDuration, int frameCount,Guid spriteId, float shipHitAnimationDelay) : base("HeroShip/Destroyed", spritePosition, frameDuration, frameCount, spriteId)
    {
        this.ShipHitAnimationDelay = shipHitAnimationDelay;
    }

    public override void Update(GameTime gameTime, GraphicsDevice gd,ConcurrentDictionary<Guid,ISprite> sprites)
    {
        AnimateSprite(gameTime, gd);
        CheckIfHitAnimationIsDone(gameTime,sprites);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        this._Sprite = new Rectangle(_CurrentFrame * _FrameWidth, 0, _FrameWidth, _FrameHeight);
        spriteBatch.Draw((_SpriteTexture), _SpritePosition, this._Sprite, Color.White, 0, new Vector2((_SpriteTexture.Width / this._FrameCount) / 2, _SpriteTexture.Height / 2), 1, SpriteEffects.None, 0);
    }
    private void CheckIfHitAnimationIsDone(GameTime gameTime, ConcurrentDictionary<Guid,ISprite> sprites)
    {
        ShipHitElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (ShipHitElapsed >= ShipHitAnimationDelay)
        {
            sprites.Remove(this.GetGuid(),out ISprite _);
        }
    }
}