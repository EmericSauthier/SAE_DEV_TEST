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
            _pingouin = new Pingouin(LARGEUR_FENETRE/2, 500 + (HAUTEUR_FENETRE/2));

            // Ennemis
            _fox1 = new MonstreRampant(new Vector2(LARGEUR_FENETRE/2, 0),"fox" , 1, 2.5);

            // Camera
            _scale = (float)0.5;
            _camera = new Camera();
            _camera.Initialize(Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);
            
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
            _pingouin.Perso = new AnimatedSprite(spriteSheet);

            // Ennemis
            SpriteSheet foxSprite = Content.Load<SpriteSheet>("fox.sf", new JsonContentLoader());
            _fox1.LoadContent(foxSprite);

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

            // Ennemis
            _chronoDep += deltaSeconds;
            _fox1.RightLeftMove(ref _chronoDep);

            //fox1.Position = new Vector2(camera1.CameraPosition.X - 100, camera1.CameraPosition.Y - 100);
            _fox1.MonsterSprite.Update(deltaSeconds);

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

            // Render Map With Camera
            _tiledMapRenderer.Draw(_camera.OrthographicCamera.GetViewMatrix());

            SpriteBatch.Begin(transformMatrix: _camera.OrthographicCamera.GetViewMatrix());

            // Pingouin
            SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position, 0, new Vector2(_scale, _scale));
            SpriteBatch.DrawPoint(_pingouin.Position.X-50*_scale, _pingouin.Position.Y + 60*_scale, Color.Green, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * _scale, _pingouin.Position.Y + 60 * _scale, Color.Green, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * _scale, _pingouin.Position.Y + 50 * _scale, Color.Red, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * _scale, _pingouin.Position.Y - 50 * _scale, Color.Red, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * _scale, _pingouin.Position.Y + 50 * _scale, Color.Blue, 5);
            SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * _scale, _pingouin.Position.Y - 50 * _scale, Color.Blue, 5);

            // Chrono
            SpriteBatch.DrawString(police, $"Chrono : {(int)_chrono}", _positionChrono, Color.White);

            // Ennemis
            SpriteBatch.Draw(_fox1.MonsterSprite, _fox1.Position, 0, new Vector2(4, 4));

            SpriteBatch.End();

            base.Draw(gameTime);
        }
        public bool CheckCollision()
        {
            ushort bottomLeft = (ushort)((_pingouin.Position.X - 50 * _scale) / _mapLayer.TileWidth);
            ushort bottomRight = (ushort)((_pingouin.Position.X + 50 * _scale) / _mapLayer.TileWidth);
            ushort y = (ushort)((_pingouin.Position.Y + 60 * _scale) / _mapLayer.TileHeight);
            TiledMapTile? tileRight;
            TiledMapTile? tileLeft;

            if ((_mapLayer.TryGetTile(bottomLeft, y, out tileRight) != false && !tileRight.Value.IsBlank) || (_mapLayer.TryGetTile(bottomRight, y, out tileLeft) != false && !tileLeft.Value.IsBlank))
                return true;

            return false;
        }

        public void Gravity()
        {
            if (!CheckCollision())
            {
                _pingouin.Fly = true;
                _pingouin.Position += new Vector2(0, 1);
            } else
            {
                _pingouin.Fly = false;
            }
        }
    }
}