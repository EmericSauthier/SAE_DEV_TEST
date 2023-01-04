using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Projet
{
    public class Game1 : Game
    {
        private const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        private OrthographicCamera _camera;
        private Vector2 _cameraPosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Window.Title = "Test";

            // Fenetre
            _graphics.PreferredBackBufferWidth = LARGEUR_FENETRE;
            _graphics.PreferredBackBufferHeight = HAUTEUR_FENETRE;
            _graphics.ApplyChanges();

            // Camera
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);
            _camera = new OrthographicCamera(viewportAdapter);
            _cameraPosition = new Vector2(LARGEUR_FENETRE/2, HAUTEUR_FENETRE + 80);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Map
            _tiledMap = Content.Load<TiledMap>("snowmap1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // Map
            _tiledMapRenderer.Update(gameTime);

            // Camera
            MoveCamera(gameTime);
            _camera.LookAt(_cameraPosition);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Camera
            _tiledMapRenderer.Draw(_camera.GetViewMatrix());

            base.Draw(gameTime);
        }

        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }

            // Can't normalize the zero vector so test for it before normalizing
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;
        }

        private void MoveCamera(GameTime gameTime)
        {
            var speed = 600;
            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();
            _cameraPosition += speed * movementDirection * seconds;
        }
    }
}