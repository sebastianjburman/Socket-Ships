using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SocketShipsClient.Sprites;

namespace SocketShipsClient;
    public class SpriteManager
    {
        private static SpriteManager _Instance;
        private List<AnimatedSprite> _Sprites;

        private SpriteManager()
        {
            _Sprites = new List<AnimatedSprite>();
            //Add Sprites to list
            SpaceShip spaceShiptest = new SpaceShip("HeroShip/Move", new Vector2(90, 300), .05, 6);
            _Sprites.Add(spaceShiptest);

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
            foreach (AnimatedSprite sprite in _Sprites)
            {
                sprite.LoadContent(contentManager);        
            }
        }
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            foreach (AnimatedSprite sprite in _Sprites)
            {
                sprite.Update(gameTime,graphicsDevice);         
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (AnimatedSprite sprite in _Sprites)
            {
               sprite.Draw(spriteBatch); 
            }
        }
    }