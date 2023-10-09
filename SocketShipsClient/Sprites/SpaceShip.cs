using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SocketShipsClient.Sprites;

public class SpaceShip:AnimatedSprite
{
    private bool BulletSide;
    private float BulletSpawnDelay = .3f;
    private float BulletSpawnTimeElapsed;
    private bool IsSpacePressed;
    private float ShipSpeed = 300;
    public SpaceShip(string spriteTextureFileName, Vector2 spritePosition, double frameDuration, int frameCount) : base(spriteTextureFileName, spritePosition, frameDuration, frameCount)
    {
    }

    public override void Update(GameTime gameTime, GraphicsDevice gd)
    {
        AnimateSprite(gameTime,gd);
        FireBullet(gameTime);
        MoveShip(gameTime,gd);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRectangle = new Rectangle(_CurrentFrame * _FrameWidth, 0, _FrameWidth, _FrameHeight);
        spriteBatch.Draw((_SpriteTexture), _SpritePosition, sourceRectangle, Color.White,0,new Vector2((_SpriteTexture.Width/this._FrameCount)/2,_SpriteTexture.Height/2),1,SpriteEffects.None,0);
    }

    private void FireBullet(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
    
        if (keyboardState.IsKeyDown(Keys.Space) && !IsSpacePressed)
        {
            IsSpacePressed = true;
            BulletSpawnTimeElapsed = 0;
        }

        if (IsSpacePressed)
        {
            BulletSpawnTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (BulletSpawnTimeElapsed >= BulletSpawnDelay)
            {
                //Shoot right Barrel
                float bulletYAxis = this._SpritePosition.Y + 16;
                if (BulletSide)
                {
                    //Shoot left barrel
                    bulletYAxis = this._SpritePosition.Y - 43;
                }
                HeroBullet heroBullet = new HeroBullet("HeroShip/HeroBullet", new Vector2(this._SpritePosition.X+60,bulletYAxis));
                SpriteManager.GetInstance(new ContentManager(new ServiceContainer())).SpawnSprite(heroBullet);        
                //Flip barrel
                this.BulletSide = !BulletSide;
                IsSpacePressed = false;
            }
        }
    }

    private void MoveShip(GameTime gameTime,GraphicsDevice gd)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        //If up key and not out of bound
        if (keyboardState.IsKeyDown(Keys.Up) && (!(_SpritePosition.Y - (_SpriteTexture.Height/3) <= gd.Viewport.Y)))
        {
            _SpritePosition.Y -= ShipSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;        
        }
        //If down key and not out of bound
        if(keyboardState.IsKeyDown(Keys.Down)&& (_SpritePosition.Y <= gd.Viewport.Height - (_SpriteTexture.Height/3)))
        {
            _SpritePosition.Y += ShipSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}