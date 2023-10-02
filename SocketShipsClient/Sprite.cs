using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SocketShipsClient;

public abstract class Sprite:ISprite
{
   protected Texture2D _SpriteTexture;
   protected Vector2 _SpritePosition;
   protected string _SpriteTextureFileName;
   
   protected Sprite(string spriteTextureFileName, Vector2 spritePosition)
   {
      _SpriteTextureFileName = spriteTextureFileName;
      _SpritePosition = spritePosition;
   }
   public void LoadContent(ContentManager cm)
   {
      this._SpriteTexture = cm.Load<Texture2D>(_SpriteTextureFileName);
   }
   public abstract void Update(GameTime gameTime, GraphicsDevice gd);
   public abstract void Draw(SpriteBatch spriteBatch);
}