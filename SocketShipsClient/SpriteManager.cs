using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SocketShipsClient.Sprites;

namespace SocketShipsClient;
    public class SpriteManager
    {
        private static SpriteManager _Instance;
        private List<ISprite> _Sprites;

        private SpriteManager()
        {
            _Sprites = new List<ISprite>();
            //Add Sprites to list
            SpaceShip spaceShiptest = new SpaceShip("HeroShip/Move", new Vector2(90, 300), .05, 6);
            HeroBullet heroBullet = new HeroBullet("HeroShip/HeroBullet", new Vector2(200, 300));
            _Sprites.Add(spaceShiptest);
            _Sprites.Add(heroBullet);

        }

        public static SpriteManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new SpriteManager();
            }
            return _Instance;
        }
        public void LoadContent(ContentManager contentManager)
        {
            foreach (ISprite sprite in _Sprites)
            {
                sprite.LoadContent(contentManager);        
            }
        }
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            foreach (ISprite sprite in _Sprites)
            {
                sprite.Update(gameTime,graphicsDevice);         
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ISprite sprite in _Sprites)
            {
               sprite.Draw(spriteBatch); 
            }
        }
    }