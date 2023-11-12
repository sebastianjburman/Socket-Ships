using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Text.Json;
using SocketShipsClient.Models;

namespace SocketShipsClient;

public abstract class Sprite:ISprite
{
   protected Texture2D _SpriteTexture;
   protected Rectangle _Sprite;
   protected Vector2 _SpritePosition;
   protected string _SpriteTextureFileName;
   protected Guid SpriteId;
   
   protected Sprite(string spriteTextureFileName, Vector2 spritePosition)
   {
      _SpriteTextureFileName = spriteTextureFileName;
      _SpritePosition = spritePosition;
      SpriteId = Guid.NewGuid();
   }
   public void LoadContent(ContentManager cm)
   {
      this._SpriteTexture = cm.Load<Texture2D>(_SpriteTextureFileName);
      this._Sprite = new Rectangle((int)_SpritePosition.X, (int)_SpritePosition.Y, this._SpriteTexture.Width, this._SpriteTexture.Height);
   }
   public abstract void Update(GameTime gameTime, GraphicsDevice gd);
   public abstract void Draw(SpriteBatch spriteBatch);
   public void SyncUp()
   {
      string data = JsonSerializer.Serialize(new SpriteSyncModel(this.SpriteId, this.GetType().Name, _SpritePosition.X,_SpritePosition.Y));
      SpriteSync.SendToServer(data);
   }
}