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
        private Regle _regle;
        public static SpriteFont police; //police pour le texte
            //BOOLEEN POUR SAVOIR SI L'ON VA SUR UNE AUTRE SCENE
        public bool clicMenu;
        public bool clicDead;
        public bool clicArret;
        public bool clicRegle;

        private Camera camera1;
        private Pingouin pingouin1;

        MonstreRampant[] monstresRampants;
        MonstreRampant fox1;

        // GameManager
        private bool gameOver;
        private KeyboardState _keyboardState;
        private TiledMapTileLayer _mapLayer;
        // Chrono
        private float _chrono;
        private Vector2 _positionChrono;
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

            // Champs
            //Champ.Initialize();

            // GameManager
            gameOver = false;

            // Fenetre
            _graphics.PreferredBackBufferWidth = LARGEUR_FENETRE;
            _graphics.PreferredBackBufferHeight = HAUTEUR_FENETRE;
            _graphics.ApplyChanges();

            // Chrono
            _chrono = 0;
            _positionChrono = new Vector2(LARGEUR_FENETRE - 200, 0);

            // Pingouin
            pingouin1 = new Pingouin(LARGEUR_FENETRE/2, HAUTEUR_FENETRE/2);

            // Ennemis
            fox1 = new MonstreRampant(new Vector2(LARGEUR_FENETRE - 200, 0),"fox" , 1);

            // Camera
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

            // Ennemis
            SpriteSheet foxSprite = Content.Load<SpriteSheet>("fox.sf", new JsonContentLoader());
            fox1.LoadContent(foxSprite);

            //Load des differente classes
            _gameOver = new GameOver(this);
            _win = new Win(this);
            _menu = new Menu(this);
            _choixNiveau = new ChoixNiveau(this);
            _regle = new Regle(this);

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
            ushort x = (ushort)(pingouin1.Position.X / _tiledMap.TileWidth);
            ushort y = (ushort)((pingouin1.Position.Y + 60) / _tiledMap.TileHeight);
            if (!IsCollision(x, y))
            {
                pingouin1.Position += new Vector2(0, 1);
            }
            pingouin1.Animate(gameOver, _keyboardState);
            pingouin1.Perso.Update(deltaSeconds);

            // Chrono
            _chrono += deltaSeconds;

            // Ennemis
            _chronoDep += deltaSeconds;
            fox1.RightLeftMove(ref _chronoDep);
            fox1.Position = new Vector2(camera1.CameraPosition.X - 100, camera1.CameraPosition.Y - 100);
            fox1.MonsterSprite.Update(deltaSeconds);

            //CHAMNGEMENT DE SCENE
            KeyboardState keyboardState = Keyboard.GetState();
                //CONDITION POUR ALLER SUR LE MENU DU JEU
            if (keyboardState.IsKeyDown(Keys.Tab) || clicMenu)
            {
                clicMenu = false;
                _screenManager.LoadScreen(_menu, new FadeTransition(GraphicsDevice, Color.Black));
            }
                //CONDITION POUR ALLER A LA SCENE WIN
            else if (_keyboardState.IsKeyDown(Keys.A))
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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Map
            _tiledMapRenderer.Draw(camera1.OrthographicCamera.GetViewMatrix());

            SpriteBatch.Begin();
            // Pingouin
            SpriteBatch.Draw(pingouin1.Perso, pingouin1.Position);
            SpriteBatch.DrawPoint(pingouin1.Position.X, pingouin1.Position.Y + 60, Color.Green, 5);
            // Chrono
            SpriteBatch.DrawString(Champ.police, $"Chrono : {(int)_chrono}", _positionChrono, Color.White);
            // Ennemis
            SpriteBatch.Draw(fox1.MonsterSprite, fox1.Position, 0, new Vector2(4, 4)); 
            SpriteBatch.End();

            base.Draw(gameTime);
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