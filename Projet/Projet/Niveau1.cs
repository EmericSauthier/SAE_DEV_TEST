using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Projet
{
    internal class Niveau1 : GameScreen
    {
        private Game1 _myGame;

        private const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;
        private GraphicsDeviceManager _graphics;
        //SOURIS POUR GERER CLIC
        private MouseState _mouseState;

        //MAP
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;


        private Camera _camera;
        private float _scale;
        private Pingouin _pingouin;

        MonstreRampant[] _monstresRampants;
        MonstreRampant _fox1;

        // GameManager
        private bool gameOver;
        private KeyboardState _keyboardState;
        private TiledMapTileLayer _mapLayer;

        // Chrono
        private float _chrono;
        private Vector2 _positionChrono;
        private float _chronoDep;


        public Niveau1(Game1 game) : base(game)
        {
            _myGame = game;

        }
        public override void Initialize()
        {

            // Chrono
            _chrono = 0;
            _positionChrono = new Vector2(LARGEUR_FENETRE - 200, 0);

            // Pingouin
            _pingouin = new Pingouin(LARGEUR_FENETRE / 2, HAUTEUR_FENETRE / 6);

            // Ennemis
            _fox1 = new MonstreRampant(new Vector2(LARGEUR_FENETRE / 2, 0), "fox", 1, 2.5);
            
            
            // Camera
            _scale = (float)0.5;
            _camera = new Camera();
            _camera.Initialize(Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);
            
            base.Initialize();
        }
        public override void LoadContent()
        {
            // Map
            _tiledMap = Content.Load<TiledMap>("snowmap1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Ground");

            // Pingouin
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            _pingouin.Perso = new AnimatedSprite(spriteSheet);

            // Ennemis
            SpriteSheet foxSprite = Content.Load<SpriteSheet>("fox.sf", new JsonContentLoader());
            _fox1.LoadContent(foxSprite);

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            // Map
            _tiledMapRenderer.Update(gameTime);
            
            // Camera
            _camera.Update(gameTime, _pingouin);

            // GameManager
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Pingouin
            ushort x = (ushort)(_pingouin.Position.X / _tiledMap.TileWidth);
            ushort y = (ushort)((_pingouin.Position.Y + 60 * _scale) / _tiledMap.TileHeight);
            if (!IsCollision(x, y))
            {
                _pingouin.Position += new Vector2(0, 1);
            }

            _pingouin.Animate(gameOver, _keyboardState);
            _pingouin.Perso.Update(deltaSeconds);

            // Chrono
            _chrono += deltaSeconds;

            // Ennemis
            _chronoDep += deltaSeconds;
            _fox1.RightLeftMove(ref _chronoDep);
            //fox1.Position = new Vector2(camera1.CameraPosition.X - 100, camera1.CameraPosition.Y - 100);
            _fox1.MonsterSprite.Update(deltaSeconds);

        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Gray);
            // Render Map With Camera
            _tiledMapRenderer.Draw(_camera.OrthographicCamera.GetViewMatrix());

            _myGame.SpriteBatch.Begin();
            // Pingouin
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position, 0, new Vector2(_scale, _scale));
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X, _pingouin.Position.Y + 60 * _scale, Color.Green, 5);

            // Chrono
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono : {(int)_chrono}", _positionChrono, Color.White);

            // Ennemis
            _myGame.SpriteBatch.Draw(_fox1.MonsterSprite, _fox1.Position, 0, new Vector2(4, 4));

            _myGame.SpriteBatch.End();
        }

        private bool IsCollision(ushort x, ushort y)
        {
            // définition de tile qui peut être null (?)
            TiledMapTile? tile;
            if (_mapLayer.TryGetTile(x, y, out tile) == false)
                return false;
            return !tile.Value.IsBlank;
        }
    }
}
