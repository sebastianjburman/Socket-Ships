using System;
using System.ComponentModel.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SocketShipsClient.Sprites;

public class HeroShip : AnimatedSprite
{
    private bool BulletSide;
    private float BulletSpawnDelay = .3f;
    private float BulletSpawnTimeElapsed;
    private bool IsSpacePressed;
    private float ShipSpeed = 650;
    private bool IsPlayer;
    private Color _color;
    private bool ShipHit;
    private float ShipHitAnimationDelay = 2.1f;
    private float ShipHitElapsed;

    public HeroShip(string spriteTextureFileName, Vector2 spritePosition, double frameDuration, int frameCount,bool isPlayer,Guid spriteId) : base(spriteTextureFileName, spritePosition, frameDuration, frameCount, spriteId)
    {
        this.IsPlayer = isPlayer;
    }

    public override void Update(GameTime gameTime, GraphicsDevice gd,ConcurrentDictionary<Guid,ISprite> sprites)
    {
        if (!ShipHit)
        {
            AnimateSprite(gameTime, gd);
            if (IsPlayer)
            {
                FireBullet(gameTime);
                MoveShip(gameTime, gd);
            }
        }

        CheckIfHitAnimationIsDone(gameTime);
        CheckForCollison(sprites);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!ShipHit)
        {
            this._Sprite = new Rectangle(_CurrentFrame * _FrameWidth, 0, _FrameWidth, _FrameHeight);
            spriteBatch.Draw((_SpriteTexture), _SpritePosition, this._Sprite, _color, 0,
                new Vector2((_SpriteTexture.Width / this._FrameCount) / 2, _SpriteTexture.Height / 2), 1,
                SpriteEffects.None, 0);
        }
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
                float bulletYAxis = this._SpritePosition.Y + 26;
                if (BulletSide)
                {
                    //Shoot left barrel
                    bulletYAxis = this._SpritePosition.Y - 26;
                }
                HeroBullet heroBullet = new HeroBullet("HeroShip/HeroBullet", new Vector2(this._SpritePosition.X + 60, bulletYAxis),Guid.NewGuid());
                SpriteManager.GetInstance(new ContentManager(new ServiceContainer())).SpawnSprite(heroBullet);
                heroBullet.SyncUp(false);
                //Flip barrel
                this.BulletSide = !BulletSide;
                IsSpacePressed = false;
            }
        }
    }

    private void MoveShip(GameTime gameTime, GraphicsDevice gd)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        //If up key and not out of bound
        if (keyboardState.IsKeyDown(Keys.Up) && (!(_SpritePosition.Y -20 - (_SpriteTexture.Height / 3) <= gd.Viewport.Y)))
        {
            _SpritePosition.Y -= ShipSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Sync new position
            SyncUp(false);
        }
        //If down key and not out of bound
        if (keyboardState.IsKeyDown(Keys.Down) && (_SpritePosition.Y +20 <= (gd.Viewport.Height) - (_SpriteTexture.Height / 3)))
        {
            _SpritePosition.Y += ShipSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //Sync new position
            SyncUp(false);
        }
    }

    private void CheckForCollison(ConcurrentDictionary<Guid,ISprite> sprites)
    {
        if (!ShipHit)
        {
            _color = Color.White;
            foreach (KeyValuePair<Guid, ISprite> sprite in sprites)
            {
                string x = sprite.Value.GetType().Name;
                if (sprite.Value.GetGuid() != this.GetGuid() && x.Equals("VillainBullet"))
                {
                    bool intersects = this.GetSpritRectangle().Intersects(sprite.Value.GetSpritRectangle());
                    if (intersects)
                    {
                        //Remove bullet that hit ship
                        sprites.TryRemove(sprite);
                        //Start timer for ship destroyed animation
                        ShipHit = true;
                        HeroShipDestroyed shiptestDestroyed = new HeroShipDestroyed(new Vector2(this._SpritePosition.X, this._SpritePosition.Y), .10, 21,Guid.NewGuid(),2.1f);
                        SpriteManager.GetInstance(new ContentManager(new ServiceContainer())).SpawnSprite(shiptestDestroyed);
                        shiptestDestroyed.SyncUp(false);
                    }

                }
            }
        }
    }

    private void CheckIfHitAnimationIsDone(GameTime gameTime)
    {
        if (ShipHit)
        {
            ShipHitElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (ShipHitElapsed >= ShipHitAnimationDelay)
            {
                ShipHit = false;
                ShipHitElapsed = 0;
            }
        }
    }
}