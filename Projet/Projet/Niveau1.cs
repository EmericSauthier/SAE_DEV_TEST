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
using System;

namespace Projet
{
    internal class Niveau1 : GameScreen
    {
        private Game1 _myGame;

        public const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;
        private GraphicsDeviceManager _graphics;

        //SOURIS POUR GERER CLIC
        private MouseState _mouseState;

        //MAP
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        //JEU
        private Camera _camera;
        public static float scale;

        // ENTITE
        private Pingouin _pingouin;
        public int _largeurPingouin = 128, _hauteurPingouin = 128, _pingouinLife;
        // Fox
        MonstreRampant[] _monstresRampants;
        MonstreRampant _fox1;
        public int _largeurFox1 = 19, _hauteurFox1 = 14;

        // Traps
        Trap _ceilingTrap1;
        private float _chronoTrap1;
        public static bool _canCollidingTrap;
        public int _largeurTrap1 = 64, _hauteurTrap1 = 64;

        // GameManager
        private bool _gameOver;
        private KeyboardState _keyboardState;
        private TiledMapTileLayer _mapLayer;

        // Chrono
        private Vector2 _positionChrono;
        private float _chrono;
        private float _chronoDep;


        public Niveau1(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _myGame.Window.Title = "Jeu du pingouin";

            // GameManager
            _gameOver = false;

            // Pingouin
            _pingouin = new Pingouin(LARGEUR_FENETRE / 2, 500 + (HAUTEUR_FENETRE / 2));
            _pingouinLife = 3; // à déplacer ?
            if (_myGame.reprendre)
            {
                _pingouin = new Pingouin(_myGame._dernierePosiPingouin.X, _myGame._dernierePosiPingouin.Y);
                _myGame.reprendre = false;
            }
            else
            {
                _pingouin = new Pingouin(LARGEUR_FENETRE / 2, 500 + (HAUTEUR_FENETRE / 2));
            }

            // Ennemis
            _fox1 = new MonstreRampant(new Vector2(1150, 850), "fox", 1, 2.5);

            // Traps
            _ceilingTrap1 = new Trap(new Vector2(300, 870));

            // Camera
            scale = (float)0.5;
            _camera = new Camera();
            _camera.Initialize(_myGame.Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);

            // Chrono
            _chrono = 0;
            _chronoDep = 0;

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

            // Traps
            SpriteSheet ceilingTrapSprite = Content.Load<SpriteSheet>("ceilingTrap.sf", new JsonContentLoader());
            _ceilingTrap1.LoadContent(ceilingTrapSprite);

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //CONDITION POUR ALLER SUR LE MENU DU JEU
            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                _myGame.pause = true;
            }
            else if (!_myGame.pause || _myGame.reprendre)
            {
                // Map
                _tiledMapRenderer.Update(gameTime);

                // Camera
                _camera.Update(gameTime, _pingouin);

                // GameManager
                _keyboardState = Keyboard.GetState();
                float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Pingouin
                _myGame._dernierePosiPingouin = new Vector2(_pingouin.Position.GetHashCode()); //envoie dans game 1 la position ddu pingouin pour pouvoir reprendre a la meme position
                _pingouin.Animate(_gameOver, _keyboardState, _mapLayer);
                _pingouin.Perso.Update(deltaSeconds);

                // Chrono
                _chrono += deltaSeconds;
                _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

                // Ennemis
                _chronoDep += deltaSeconds;
                _fox1.RightLeftMove(ref _chronoDep);
                _fox1.Sprite.Update(deltaSeconds);

            // Traps
            _chronoTrap1 += deltaSeconds;
            _ceilingTrap1.PressActivation(ref _chronoTrap1, ref _canCollidingTrap);
            if(Collision.IsCollidingTrap(_pingouin, _largeurPingouin, _hauteurPingouin, _ceilingTrap1, _largeurTrap1, _hauteurTrap1, _scale, _canCollidingTrap))
            {
                _myGame.clicDead = true;
            }
            _ceilingTrap1.Sprite.Update(deltaSeconds);
        }

        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Render Map With Camera
            _tiledMapRenderer.Draw(_camera.OrthographicCamera.GetViewMatrix());

            _myGame.SpriteBatch.Begin(transformMatrix: _camera.OrthographicCamera.GetViewMatrix());

            // Pingouin
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position, 0, new Vector2(scale));

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 40 * scale, _pingouin.Position.Y + 60 * scale, Color.Green, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 40 * scale, _pingouin.Position.Y + 60 * scale, Color.Green, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 40 * scale, _pingouin.Position.Y - 60 * scale, Color.Green, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 40 * scale, _pingouin.Position.Y - 60 * scale, Color.Green, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y + 50 * scale, Color.Red, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y, Color.Red, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y - 50 * scale, Color.Red, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y + 50 * scale, Color.Blue, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y, Color.Blue, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y - 50 * scale, Color.Blue, 5);

            // Chrono
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono : {(int)_chrono}", _positionChrono, Color.White);
            //_myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Trap : {Math.Round(_chronoTrap1, 2)}", _positionChrono + new Vector2(-100, 50), Color.White);

            // Ennemis
            _myGame.SpriteBatch.Draw(_fox1.Sprite, _fox1.Position, 0, new Vector2(3, 3));

            // Traps
            _myGame.SpriteBatch.Draw(_ceilingTrap1.Sprite, _ceilingTrap1.Position, 0, new Vector2(1, 1));

            _myGame.SpriteBatch.End();
        }
    }
}
