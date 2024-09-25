using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

using System.Collections.Generic;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;




namespace Tilemap_Candy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
//------------------add---------------------
        const int MapWidth = 1600;
        const int MapHeight = 900;
        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        TiledMapObjectLayer _platformTiledObj;
        private readonly List<IEntity> _entities = new List<IEntity>();
        public readonly CollisionComponent _collisionComponent;
        //------------------------------------------
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, MapWidth, MapHeight));
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
//--------------------add-------------------

            _graphics.PreferredBackBufferWidth = MapWidth; // Set the width of window
            _graphics.PreferredBackBufferHeight = MapHeight; // Set the height of window
            _graphics.ApplyChanges();

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //------------------------------------------
            base.Initialize();
        }

        protected override void LoadContent()
        {
         
 //------------- Load tilemap --------------------------------
            _tiledMap = Content.Load<TiledMap>("Map_Candy");

            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            //Get object layers
            foreach (TiledMapObjectLayer layer in _tiledMap.ObjectLayers)
            {
                if (layer.Name == "Wall")
                {
                    _platformTiledObj = layer;
                }
            }
            foreach (TiledMapObject obj in _platformTiledObj.Objects)
            {
                Vector2 position = new Vector2(obj.Position.X, obj.Position.Y);
                _entities.Add(new PlatformEntity(this, new RectangleF(position, obj.Size)));
            }
            //Setup player
            _entities.Add(new PlayerEntity(this, new RectangleF(new Vector2(1504, 417), new Vector2(64,64))));

            foreach (IEntity entity in _entities)
            {
                _collisionComponent.Insert((ICollisionActor)entity);


            }
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // -------------------------------------------------------------  
        }

        protected override void Update(GameTime gameTime)    
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //--------------------add-------------------
            foreach (IEntity entity in _entities)
            {
                entity.Update(gameTime);
            }
            _collisionComponent.Update(gameTime);
            _tiledMapRenderer.Update(gameTime);
//-------------------------------------------
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
//--------------------add-------------------
            _tiledMapRenderer.Draw();
            _spriteBatch.Begin();
            foreach (IEntity entity in _entities)
            {
                entity.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            //-------------------------------------------
            base.Draw(gameTime);
        }
//------------------add--------------
        public int GetMapWidth()
        {
            return MapWidth;
        }
        public int GetMapHeight()
        {
            return MapHeight;
        }
        //-------------------------------------------
    }
}
