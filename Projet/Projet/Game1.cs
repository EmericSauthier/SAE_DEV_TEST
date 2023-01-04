﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.TextureAtlases;

namespace Projet
{
    public class Game1 : Game
    {
        private const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;

        private GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch { get; set; }

        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        private readonly ScreenManager _screenManager;

        private GameOver _gameOver;
        private Win _win;
        private Menu _menu;
        private ChoixNiveau _choixNiveau;

        //CHAMPS POUR MENU
        public string regle;
        public Vector2 positionRegle;
        public string jouer;
        public Vector2 positionJouer;
        public string niv;
        public Vector2 positionNiv;
        public SpriteFont police;
        public string quitter;
        public Vector2 positionQuitter;

        private Camera camera1;
        private Pingouin pingouin1;

        // GameManager
        private bool gameOver;
        private KeyboardState _keyboardState;

        //CHAMPS POUR WIN
        public string messageGagner;
        public Vector2 positionMessageGagner;
        public string messageNivSuiv;
        public Vector2 positionMessageNivSuiv;
        public string messageMenu;
        public Vector2 positionMessageMenu;

        //CHAMPS POUR GAMEOVER
        public string messagePerdu;
        public Vector2 positionMessagePerdu;
        public string messageRejouer;
        public Vector2 positionMessageRejouer;

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

            //CHAMPS POUR MENU
            regle = "Notes de pingouin";
            jouer = "Jouer";
            niv = "Charger un niveau";
            quitter = "Quitter";
            float tailleRegle = 24 * regle.Length;
            positionRegle = new Vector2(tailleRegle / 2, 50);
            positionJouer = new Vector2(tailleRegle / 2, 150);
            positionNiv = new Vector2(tailleRegle / 2, 250);
            positionNiv = new Vector2(tailleRegle / 2, 250);
            positionQuitter = new Vector2(tailleRegle / 2, 350);

            //CHAMPS POUR WIN
            messageGagner = "Niveau completer !";
            messageMenu = "Menu";
            messageNivSuiv = "Niveau suivant -->";
            positionMessageGagner = new Vector2(50, 50);
            positionMessageMenu = new Vector2(50, 350);
            positionMessageNivSuiv = new Vector2(250, 350);

            //CHAMPS POUR GAMEOVER
            messagePerdu = "C'est mort...";
            messageRejouer = "Reessayer";
            positionMessagePerdu = new Vector2(50, 50);
            positionMessageRejouer = new Vector2(50, 350);

            // GameManager
            gameOver = false;


            // Fenetre
            _graphics.PreferredBackBufferWidth = LARGEUR_FENETRE;
            _graphics.PreferredBackBufferHeight = HAUTEUR_FENETRE;
            _graphics.ApplyChanges();

            pingouin1 = new Pingouin(LARGEUR_FENETRE/2, HAUTEUR_FENETRE/2);

            camera1 = new Camera();
            camera1.Initialize(Window, GraphicsDevice, LARGEUR_FENETRE/2, HAUTEUR_FENETRE/2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Map
            _tiledMap = Content.Load<TiledMap>("snowmap1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            // Pingouin
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            pingouin1.Perso = new AnimatedSprite(spriteSheet);

            //Load des differente classes
            _gameOver = new GameOver(this);
            _win = new Win(this);
            _menu = new Menu(this);
            _choixNiveau = new ChoixNiveau(this);

            //POUR MENU
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
            camera1.Update(gameTime, pingouin1);

            // GameManager
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Pingouin
            pingouin1.Animate(gameOver, _keyboardState);
            pingouin1.Perso.Update(deltaSeconds);
            

            //CHAMNGEMENT DE SCENE
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Tab) || _win.clicMenu || _gameOver.clicMenu)
            {
                _win.clicMenu = false;
                _gameOver.clicMenu = false;
                _screenManager.LoadScreen(_menu, new FadeTransition(GraphicsDevice, Color.Black));
            }
            else if (_keyboardState.IsKeyDown(Keys.A))
            {
                _screenManager.LoadScreen(_win, new FadeTransition(GraphicsDevice, Color.Black));
            }
            else if (_menu.clicChoixNiv)
            {
                _menu.clicChoixNiv = false;
                _screenManager.LoadScreen(_choixNiveau, new FadeTransition(GraphicsDevice, Color.Black));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Pingouin
            SpriteBatch.Begin();
            SpriteBatch.Draw(pingouin1.Perso, pingouin1.Position);
            SpriteBatch.End();

            // Camera
            _tiledMapRenderer.Draw(camera1.OrthographicCamera.GetViewMatrix());

            base.Draw(gameTime);
        }
    }
}