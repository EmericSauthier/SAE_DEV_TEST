using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace Projet
{
    public class Game1 : Game
    {
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
            Window.Title = "Test";

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
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //_tiledMap = Content.Load<TiledMap>("map");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

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
            _tiledMapRenderer.Update(gameTime);

            //CHAMNGEMENT DE SCENE
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Tab) || _win.clicMenu || _gameOver.clicMenu)
            {
                _screenManager.LoadScreen(_menu, new FadeTransition(GraphicsDevice, Color.Black));
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                _screenManager.LoadScreen(_win, new FadeTransition(GraphicsDevice, Color.Black));
            }
            else if (_menu.clicChoixNiv)
            {
                _screenManager.LoadScreen(_choixNiveau, new FadeTransition(GraphicsDevice, Color.Black));
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Red);

            // TODO: Add your drawing code here
            _tiledMapRenderer.Draw();

            base.Draw(gameTime);
        }
    }
}