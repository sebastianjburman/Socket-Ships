using System.Collections.Concurrent;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SocketShipsClient.Sprites;

namespace SocketShipsClient;
    public class SpriteManager
    {
        private static SpriteManager _Instance;
        private ConcurrentBag<ISprite> _Sprites;
        private ContentManager _ContentManager;

        private SpriteManager(ContentManager contentManager)
        {
            this._ContentManager = contentManager;
            _Sprites = new ConcurrentBag<ISprite>();
            SpaceShip spaceShiptest = new SpaceShip("HeroShip/Move", new Vector2(90, 300), .05, 6);
            _Sprites.Add(spaceShiptest);
        }

        public static SpriteManager GetInstance(ContentManager contentManager)
        {
            if (_Instance == null)
            {
                _Instance = new SpriteManager(contentManager);
            }
            return _Instance;
        }
        public void LoadContent()
        {
            foreach (ISprite sprite in _Sprites)
            {
                sprite.LoadContent(_ContentManager);        
            }
        }
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            foreach (ISprite sprite in _Sprites)
            {
                sprite.Update(gameTime,graphicsDevice);         
                sprite.SyncUp();
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ISprite sprite in _Sprites)
            {
               sprite.Draw(spriteBatch); 
            }
        }

        public void SpawnSprite(ISprite sprite)
        {
            sprite.LoadContent(_ContentManager);
           _Sprites.Add(sprite); 
        }
    }