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
    public class Game1 : Game
    {
        private const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;

        private GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch { get; set; }

        private readonly ScreenManager _screenManager;

        //MAP
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        //LES CLASSES EN LIEN
        private GameOver _gameOver;
        private Win _win;
        private Menu _menu;
        private ChoixNiveau _choixNiveau;
        private Regle _regle;
        private Niveau1 _niveau1;
        public static SpriteFont police; //police pour le texte
        
        //BOOLEEN POUR SAVOIR SI L'ON VA SUR UNE AUTRE SCENE
        public bool clicMenu;
        public bool clicDead;
        public bool clicArret;
        public bool clicRegle;
        public bool clicNiveau1;

        
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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            Window.Title = "Jeu du pingouin";

            // Fenetre
            _graphics.PreferredBackBufferWidth = LARGEUR_FENETRE;
            _graphics.PreferredBackBufferHeight = HAUTEUR_FENETRE;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            //Load des differente classes
            _gameOver = new GameOver(this);
            _win = new Win(this);
            _menu = new Menu(this);
            _choixNiveau = new ChoixNiveau(this);
            _regle = new Regle(this);
            _niveau1 = new Niveau1(this);

            //POLICE
            police = Content.Load<SpriteFont>("Font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            
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

            //CHAMNGEMENT DE SCENE
            KeyboardState keyboardState = Keyboard.GetState();
            
            //CONDITION POUR ALLER SUR LE MENU DU JEU
            if (keyboardState.IsKeyDown(Keys.Tab) || clicMenu)
            {
                clicMenu = false;
                _screenManager.LoadScreen(_menu, new FadeTransition(GraphicsDevice, Color.Black));
            }
                //CONDITION POUR ALLER A LA SCENE WIN
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                _screenManager.LoadScreen(_win, new FadeTransition(GraphicsDevice, Color.Black));
            }
                //CONDITION POUR ALLER A LA SCENE DU CHOIX DE NIVEAU
            else if (_menu.clicChoixNiv)
            {
                _menu.clicChoixNiv = false;
                _screenManager.LoadScreen(_choixNiveau, new FadeTransition(GraphicsDevice, Color.Black));
            }
                //CONDITION POUR METTRE LA PAGE GAME OVER
            else if (clicDead)
            {
                clicDead = false;
                _screenManager.LoadScreen(_gameOver, new FadeTransition(GraphicsDevice, Color.Black));
            }
                //CONDITION POUR FERMER LE JEU
            else if (clicArret)
            {
                Exit();
            }
            //CONDITION POUR ALLER AUX REGLE DU JEU
            else if (clicRegle)
            {
                clicRegle = false;
                _screenManager.LoadScreen(_regle, new FadeTransition(GraphicsDevice, Color.Black));
            }
            //CONDITION POUR LANCER LE NIVEAU 1
            else if (clicNiveau1)
            {
                clicNiveau1 = false;
                _screenManager.LoadScreen(_niveau1, new FadeTransition(GraphicsDevice, Color.Black));
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // Render Map With Camera
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
            
            base.Draw(gameTime);
        }

        private bool IsCollidingTrap()
        {
            Rectangle _hitBoxTrap = new Rectangle((int)_ceilingTrap1.Position.X, (int)_ceilingTrap1.Position.Y + 50, (int)(64 * _scale), (int)(14 * _scale));
            Rectangle _hitBoxPingouin = new Rectangle((int)_pingouin.Position.X, (int)_pingouin.Position.Y, (int)(128*_scale), (int)(128 * _scale));

            if (_hitBoxPingouin.Intersects(_hitBoxTrap))
            {
                return true;
            }
            else return false;
        }
    }
}