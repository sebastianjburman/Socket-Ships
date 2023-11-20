using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private Guid PlayerGuid;

        private SpriteManager(ContentManager contentManager)
        {
            this._ContentManager = contentManager;
            _Sprites = new ConcurrentDictionary<Guid, ISprite>();

            Background background = new Background("SpaceBackground", new Vector2(0,0), new Guid());
            _Sprites.TryAdd(background.GetGuid(), background);
            
            Console.WriteLine("Select Hero or Villain Ship?");
            Console.WriteLine("1:Hero");
            Console.WriteLine("2:Villain");
            int ship = Convert.ToInt32(Console.ReadLine());
            if (ship == 2)
            {
               VillainShip villainShip = new VillainShip("VillainShip/Move", new Vector2(1850, 300), .05, 6,true,Guid.NewGuid());
                _Sprites.TryAdd(villainShip.GetGuid(), villainShip);
                PlayerGuid = villainShip.GetGuid();
            }
            else
            {
                HeroShip heroShip = new HeroShip("HeroShip/Move", new Vector2(100, 300), .05, 6,true,Guid.NewGuid());
                _Sprites.TryAdd(heroShip.GetGuid(), heroShip);
                PlayerGuid = heroShip.GetGuid();
            }
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
            //Call update for every sprite 
            //If Sprite is not on the screen. If not then dispose of it
            foreach (KeyValuePair<Guid,ISprite> sprite in _Sprites)
            {
                Vector2 spritePos = sprite.Value.GetPosition();
                if (spritePos.X < 0 || spritePos.X > graphicsDevice.Viewport.Width || spritePos.Y < 0 || spritePos.Y > graphicsDevice.Viewport.Height)
                {
                    bool removed = _Sprites.TryRemove(sprite.Key,out _);
                    if (removed)
                    {
                        Console.WriteLine($"Item removed successfully.");
                    }
                }
                sprite.Value.Update(gameTime, graphicsDevice,this._Sprites);
            }

            try
            {
                //Update dictionary with sprite data coming down from the server
                SpriteSyncModel newData = await SpriteSync.ReceiveFromServer();
                Console.WriteLine($"Reciving Message {newData.Type} + {newData.RemoveSprite}");

                if (!newData.RemoveSprite)
                {
                    //Update existing sprites
                    if (_Sprites.ContainsKey(newData.GUID))
                    {
                        _Sprites.TryGetValue(newData.GUID, out ISprite sprite);
                        sprite.SetPosition(new Vector2(newData.X, newData.Y));
                    }
                    //Create new sprites 
                    else
                    {
                        switch (newData.Type)
                        {
                            case "HeroShip":
                                HeroShip newShip = new HeroShip("HeroShip/Move", new Vector2(newData.X, newData.Y), .05,
                                    6, false, newData.GUID);
                                SpawnSprite(newShip);
                                break;
                            case "VillainShip":
                                VillainShip villainShip = new VillainShip("VillainShip/Move",
                                    new Vector2(newData.X, newData.Y), .05,
                                    6, false, newData.GUID);
                                SpawnSprite(villainShip);
                                break;
                            case "HeroBullet":
                                HeroBullet heroBullet = new HeroBullet("HeroShip/HeroBullet",
                                    new Vector2(newData.X, newData.Y), newData.GUID);
                                SpawnSprite(heroBullet);
                                break;
                            case "VillainBullet":
                                VillainBullet villainBullet = new VillainBullet("VillainShip/VillainBullet",
                                    new Vector2(newData.X, newData.Y), newData.GUID);
                                SpawnSprite(villainBullet);
                                break;
                            case "VillainShipDestroyed":
                                VillainShipDestroyed villainShipDestroyed = new VillainShipDestroyed(
                                    new Vector2(newData.X, newData.Y), .10,
                                    15, newData.GUID, 1.5f);
                                SpawnSprite(villainShipDestroyed);
                                break;
                            case "HeroShipDestroyed":
                                HeroShipDestroyed heroShipDestroyed = new HeroShipDestroyed(
                                    new Vector2(newData.X, newData.Y), .10,
                                    21, newData.GUID, 2.1f);
                                SpawnSprite(heroShipDestroyed);
                                break;
                        }
                    }
                }
                else if (newData.RemoveSprite)
                {
                    bool x = this._Sprites.TryRemove(newData.GUID, out  ISprite removeSprite);
                    Console.WriteLine(removeSprite.GetGuid());
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

        public void QuitGame()
        {
            _Sprites.TryGetValue(PlayerGuid, out ISprite player);
            player.SyncUp(true);
        }
        
    }