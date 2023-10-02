using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SocketShipsClient;

public abstract class AnimatedSprite:ISprite
{
    protected Texture2D _SpriteTexture;
    protected Vector2 _SpritePosition;
    protected string _SpriteTextureFileName;
    protected int _CurrentFrame;
    protected int _FrameWidth;
    protected int _FrameHeight;
    protected double _FrameDuration;
    protected double _TimeElapsed;
    protected int _FrameCount;

    protected AnimatedSprite(string spriteTextureFileName, Vector2 spritePosition,double frameDuration, int frameCount)
    {
        this._SpriteTextureFileName = spriteTextureFileName;
        this._SpritePosition = spritePosition;
        _FrameDuration = frameDuration;
        _FrameCount = frameCount;
    }
    
   public void LoadContent(ContentManager cm)
   {
      this._SpriteTexture = cm.Load<Texture2D>(_SpriteTextureFileName);
      this._FrameWidth = this._SpriteTexture.Width / _FrameCount;
      this._FrameHeight = this._SpriteTexture.Height;
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
    public abstract void Update(GameTime gameTime, GraphicsDevice gd);
    public abstract void Draw(SpriteBatch spriteBatch);
}
