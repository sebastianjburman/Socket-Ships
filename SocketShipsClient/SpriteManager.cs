using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SocketShipsClient.Sprites;
using SocketShipsClient.Models;

namespace SocketShipsClient;
    public class SpriteManager
    {
        private static SpriteManager _Instance;
        private ConcurrentDictionary<Guid,ISprite> _Sprites;
        private ContentManager _ContentManager;

        private SpriteManager(ContentManager contentManager)
        {
            this._ContentManager = contentManager;
            _Sprites = new ConcurrentDictionary<Guid, ISprite>();
            SpaceShip spaceShiptest = new SpaceShip("HeroShip/Move", new Vector2(90, 300), .05, 6,true,Guid.NewGuid());
            _Sprites.TryAdd(spaceShiptest.GetGuid(), spaceShiptest);
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
            //Parallel.ForEach()
            foreach (KeyValuePair<Guid,ISprite> sprite in _Sprites)
            {
                sprite.Value.LoadContent(_ContentManager);        
            }
        }
        public async void  Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            foreach (KeyValuePair<Guid,ISprite> sprite in _Sprites)
            {
                Vector2 spritePos = sprite.Value.GetPosition();
                //If Sprite is not on the screen. If not then dispose of it
                if (spritePos.X < 0 || spritePos.X > graphicsDevice.Viewport.Width || spritePos.Y < 0 || spritePos.Y > graphicsDevice.Viewport.Height)
                {
                    bool removed = _Sprites.TryRemove(sprite.Key,out _);
                    if (removed)
                    {
                        Console.WriteLine($"Item removed successfully.");
                    }
                }
                sprite.Value.Update(gameTime, graphicsDevice);
            }

            try
            {
                //Update dictionary with sprite data coming down from the server
                SpriteSyncModel newData = await SpriteSync.ReceiveFromServer();

                //Update existing sprites
                if (_Sprites.ContainsKey(newData.GUID))
                {
                    switch (newData.Type)
                    {
                        case "SpaceShip":
                            _Sprites.TryGetValue(newData.GUID, out ISprite sprite);
                            sprite.SetPosition(new Vector2(newData.X, newData.Y));
                            break;
                    }
                }
                //Create new sprites 
                else
                {
                    switch (newData.Type)
                    {
                        case "SpaceShip":
                            SpaceShip newShip = new SpaceShip("HeroShip/Move", new Vector2(newData.X, newData.Y), .05,
                                6, false,newData.GUID);
                            SpawnSprite(newShip);
                            break;
                        case "HeroBullet":
                            HeroBullet heroBullet = new HeroBullet("HeroShip/HeroBullet", new Vector2(newData.X,newData.Y),newData.GUID);
                            SpawnSprite(heroBullet);
                            break; 
                    }
                }
            }
            catch
            {
                
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (KeyValuePair<Guid,ISprite> sprite in _Sprites)
            {
               sprite.Value.Draw(spriteBatch); 
            }
        }

        public void SpawnSprite(ISprite sprite)
        {
            sprite.LoadContent(_ContentManager);
            _Sprites.TryAdd(sprite.GetGuid(), sprite);
        }
        
    }