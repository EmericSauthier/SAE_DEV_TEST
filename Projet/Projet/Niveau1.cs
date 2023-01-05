﻿using Microsoft.Xna.Framework;
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

        //JEU
        private Camera _camera;
        private float _scale;
        private Pingouin _pingouin;

        MonstreRampant[] _monstresRampants;
        MonstreRampant _fox1;

        AnimatedPress _ceilingTrap1;
        private float _chronoTrap1;
        public static bool canCollidingTrap;

        // GameManager
        private bool gameOver;
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
            Window.Title = "Jeu du pingouin";

            // GameManager
            gameOver = false;

            // Pingouin
            _pingouin = new Pingouin(LARGEUR_FENETRE / 2, 500 + (HAUTEUR_FENETRE / 2));

            // Ennemis
            _fox1 = new MonstreRampant(new Vector2(1150, 850), "fox", 1, 2.5);

            // Traps
            _ceilingTrap1 = new AnimatedPress(new Vector2(300, 870));

            // Camera
            _scale = (float)0.5;
            _camera = new Camera();
            _camera.Initialize(Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);

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

            //POLICE
            police = Content.Load<SpriteFont>("Font");

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
            _pingouin.Animate(gameOver, _keyboardState);
            _pingouin.Perso.Update(deltaSeconds);
            Gravity();

            // Chrono
            _chrono += deltaSeconds;
            _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

            // Ennemis
            _chronoDep += deltaSeconds;
            _fox1.RightLeftMove(ref _chronoDep);
            _fox1.Sprite.Update(deltaSeconds);

            // Traps
            _chronoTrap1 += deltaSeconds;
            System.Diagnostics.Debug.WriteLine(_chronoTrap1);
            _ceilingTrap1.Activation(ref deltaSeconds);
            _ceilingTrap1.Sprite.Update(deltaSeconds);
            if (IsCollidingTrap())
            {
                _screenManager.LoadScreen(_gameOver, new FadeTransition(GraphicsDevice, Color.Black));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _// Render Map With Camera
            _tiledMapRenderer.Draw(_camera.OrthographicCamera.GetViewMatrix());

            SpriteBatch.Begin(transformMatrix: _camera.OrthographicCamera.GetViewMatrix());

            // Pingouin
            SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position, 0, new Vector2(_scale, _scale));

            SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * _scale, _pingouin.Position.Y + 60 * _scale, Color.Green, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * _scale, _pingouin.Position.Y + 60 * _scale, Color.Green, 5);

            SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * _scale, _pingouin.Position.Y + 50 * _scale, Color.Red, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * _scale, _pingouin.Position.Y - 50 * _scale, Color.Red, 5);

            SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * _scale, _pingouin.Position.Y + 50 * _scale, Color.Blue, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * _scale, _pingouin.Position.Y - 50 * _scale, Color.Blue, 5);

            // Chrono
            SpriteBatch.DrawString(police, $"Chrono : {(int)_chrono}", _positionChrono, Color.White);

            // Ennemis
            SpriteBatch.Draw(_fox1.Sprite, _fox1.Position, 0, new Vector2(3, 3));

            // Traps
            SpriteBatch.Draw(_ceilingTrap1.Sprite, _ceilingTrap1.Position, 0, new Vector2(1, 1));

            SpriteBatch.End();
        }

        private bool IsCollidingTrap()
        {
            Rectangle _hitBoxTrap = new Rectangle((int)_ceilingTrap1.Position.X, (int)_ceilingTrap1.Position.Y + 50, (int)(64 * _scale), (int)(14 * _scale));
            Rectangle _hitBoxPingouin = new Rectangle((int)_pingouin.Position.X, (int)_pingouin.Position.Y, (int)(128 * _scale), (int)(128 * _scale));

            if (_hitBoxPingouin.Intersects(_hitBoxTrap))
            {
                return true;
            }
            else return false;
        }
    }
    }
}
