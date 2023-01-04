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


        //LES CLASSES EN LIEN
        private GameOver _gameOver;
        private Win _win;
        private Menu _menu;
        private ChoixNiveau _choixNiveau;

        public bool clicMenu;
        public bool clicDead;

        private Camera camera1;
        private Pingouin pingouin1;

        // GameManager
        private bool gameOver;
        private KeyboardState _keyboardState;
        private TiledMapTileLayer _mapLayer;

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

            // Champs
            Champ.Initialize();

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
            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Ground");

            // Pingouin
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            pingouin1.Perso = new AnimatedSprite(spriteSheet);

            //Load des differente classes
            _gameOver = new GameOver(this);
            _win = new Win(this);
            _menu = new Menu(this);
            _choixNiveau = new ChoixNiveau(this);

            //POUR MENU
            Champ.police = Content.Load<SpriteFont>("Font");
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
            if (keyboardState.IsKeyDown(Keys.Tab) || clicMenu)
            {
                clicMenu = false;
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
            else if (clicDead)
            {
                clicDead = false;
                _screenManager.LoadScreen(_gameOver, new FadeTransition(GraphicsDevice, Color.Black));
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