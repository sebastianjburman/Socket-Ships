using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Text.Json;
using SocketShipsClient.Models;
using System.Collections.Concurrent;

namespace SocketShipsClient;

public abstract class AnimatedSprite:ISprite
{
    protected Texture2D _SpriteTexture;
    protected Rectangle _Sprite;
    protected Vector2 _SpritePosition;
    protected string _SpriteTextureFileName;
    protected int _CurrentFrame;
    protected int _FrameWidth;
    protected int _FrameHeight;
    protected double _FrameDuration;
    protected double _TimeElapsed;
    protected int _FrameCount;
    protected Guid SpriteId;

    protected AnimatedSprite(string spriteTextureFileName, Vector2 spritePosition,double frameDuration, int frameCount,Guid spriteId)
    {
        this._SpriteTextureFileName = spriteTextureFileName;
        this._SpritePosition = spritePosition;
        _FrameDuration = frameDuration;
        _FrameCount = frameCount;
        this.SpriteId = spriteId;
    }
    
   public void LoadContent(ContentManager cm)
   {
      this._SpriteTexture = cm.Load<Texture2D>(_SpriteTextureFileName);
      this._FrameWidth = this._SpriteTexture.Width / _FrameCount;
      this._FrameHeight = this._SpriteTexture.Height;
      this._Sprite = new Rectangle(_CurrentFrame * _FrameWidth, 0, _FrameWidth, _FrameHeight);
   }
    protected void AnimateSprite(GameTime gameTime, GraphicsDevice gd)
    {
        //Handle Animation
        _TimeElapsed += gameTime.ElapsedGameTime.TotalSeconds;
        if (_TimeElapsed >= _FrameDuration)
        {
            _CurrentFrame = (_CurrentFrame + 1) % (_SpriteTexture.Width / _FrameWidth);
            _TimeElapsed = 0;
        } 
    }

    public void SyncUp(bool deleteSprite)
    {
        string data = JsonSerializer.Serialize(new SpriteSyncModel(this.SpriteId, this.GetType().Name, _SpritePosition.X, _SpritePosition.Y,deleteSprite));
        SpriteSync.SendToServer(data);
    }
    public Vector2 GetPosition()
    {
        return this._SpritePosition;
    }

    public Guid GetGuid()
    {
        return this.SpriteId;
    }
    public void SetPosition(Vector2 pos)
    {
        this._SpritePosition = pos;
    }
    public Rectangle GetSpritRectangle()
    {
        return new Rectangle((int)this._SpritePosition.X , (int)this._SpritePosition.Y - (this._Sprite.Height/2), this._Sprite.Width, this._Sprite.Height);
    }
    public abstract void Update(GameTime gameTime, GraphicsDevice gd, ConcurrentDictionary<Guid,ISprite> sprites);
    public abstract void Draw(SpriteBatch spriteBatch);
}
