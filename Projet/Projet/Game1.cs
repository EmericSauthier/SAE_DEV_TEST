using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

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
        public const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;

        private GraphicsDeviceManager _graphics;
        public SpriteBatch SpriteBatch { get; set; }

        private readonly ScreenManager _screenManager;

        //LES CLASSES EN LIEN
        private GameOver _gameOver;
        private Win _win;
        private Menu _menu;
        private ChoixNiveau _choixNiveau;
        private Regle _regle;
        private Snow _snow;
        private Desert _desert;
        public static SpriteFont police; //police pour le texte
        
        //BOOLEEN POUR SAVOIR SI L'ON VA SUR UNE AUTRE SCENE
        public bool clicMenu;
        public bool goDead;
        public bool clicWin;
        public bool goStop;
        public bool goRules;
        public bool goSnow;
        public bool goDesert;
        public bool pause;
        public bool reprendre;
        public int nivActu;

        // Touche de déplacement et d'attaque
        public static Keys gauche;
        public static Keys droite;
        public static Keys sauter;
        public static Keys glisser;
        public static Keys attaquer;
        
        //SAUVEGARDE
        public Vector2 dernierePosiPingouin;

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

            // Paramétrage des touches automatique

            gauche = Keys.Left;
            droite = Keys.Right;
            sauter = Keys.Space;
            glisser = Keys.Down;
            attaquer = Keys.Enter;

            clicMenu = true;

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
            _snow = new Snow(this);
            _desert = new Desert(this);

            //POLICE
            police = Content.Load<SpriteFont>("Font/Font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Récupération des entrées
            KeyboardState keyboardState = Keyboard.GetState();
            
            // CONDITION POUR ALLER SUR LE MENU DU JEU
            if (keyboardState.IsKeyDown(Keys.Tab) || clicMenu)
            {
                clicMenu = false;
                pause = true;
                _screenManager.LoadScreen(_menu, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR ALLER A LA SCENE WIN
            else if (keyboardState.IsKeyDown(Keys.A)||clicWin)
            {
                clicWin = false;
                _screenManager.LoadScreen(_win, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR ALLER A LA SCENE DU CHOIX DE NIVEAU
            else if (_menu.clicChoixNiv)
            {
                _menu.clicChoixNiv = false;
                _screenManager.LoadScreen(_choixNiveau, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR METTRE LA PAGE GAME OVER
            else if (goDead)
            {
                goDead = false;
                _screenManager.LoadScreen(_gameOver, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR FERMER LE JEU
            else if (goStop)
            {
                Exit();
            }
            // CONDITION POUR ALLER AUX REGLE DU JEU
            else if (goRules)
            {
                goRules = false;
                _screenManager.LoadScreen(_regle, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR LANCER LE NIVEAU 1
            else if (goDesert)
            {
                goDesert = false;
                pause = false;
                _screenManager.LoadScreen(_desert, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR LANCER LE NIVEAU 2
            else if (goSnow)
            {
                goSnow = false;
                pause = false;
                _screenManager.LoadScreen(_snow, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR REPRENDRE LA PARTIE EN COURS (SI ELLE N'A PAS ETE TERMINER)
            else if (reprendre)
            {
                pause = false;
                if (nivActu==2)
                    _screenManager.LoadScreen(_snow, new FadeTransition(GraphicsDevice, Color.Black));
                else
                    _screenManager.LoadScreen(_desert, new FadeTransition(GraphicsDevice, Color.Black));
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
                       
            base.Draw(gameTime);
        }
    }
}