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
        private Niveau1 _niveau1;
        private Niveau2 _niveau2;
        private Parametres _parametres;
        public static SpriteFont police; //police pour le texte
        
        //BOOLEEN POUR SAVOIR SI L'ON VA SUR UNE AUTRE SCENE
        public bool clicMenu;
        public bool clicDead;
        public bool clicArret;
        public bool clicRegle;
        public bool clicNiveau1;
        public bool clicNiveau2;
        public bool clicParametres;
        public bool pause;
        public bool reprendre;
        
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
            _niveau1 = new Niveau1(this);
            _niveau2 = new Niveau2(this);
            _parametres = new Parametres(this);

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
            else if (keyboardState.IsKeyDown(Keys.A))
            {
                _screenManager.LoadScreen(_win, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR ALLER A LA SCENE DU CHOIX DE NIVEAU
            else if (_menu.clicChoixNiv)
            {
                _menu.clicChoixNiv = false;
                _screenManager.LoadScreen(_choixNiveau, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR METTRE LA PAGE GAME OVER
            else if (clicDead)
            {
                clicDead = false;
                _screenManager.LoadScreen(_gameOver, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR FERMER LE JEU
            else if (clicArret)
            {
                Exit();
            }
            // CONDITION POUR ALLER AUX REGLE DU JEU
            else if (clicRegle)
            {
                clicRegle = false;
                _screenManager.LoadScreen(_regle, new FadeTransition(GraphicsDevice, Color.Black));
            }// CONDITION POUR ALLER AUX PARAMETRES
            else if (clicParametres)
            {
                clicParametres = false;
                _screenManager.LoadScreen(_parametres, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR LANCER LE NIVEAU 1
            else if (clicNiveau1)
            {
                clicNiveau1 = false;
                pause = false;
                _screenManager.LoadScreen(_niveau1, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR LANCER LE NIVEAU 2
            else if (clicNiveau2)
            {
                clicNiveau2 = false;
                pause = false;
                _screenManager.LoadScreen(_niveau2, new FadeTransition(GraphicsDevice, Color.Black));
            }
            // CONDITION POUR REPRENDRE LA PARTIE EN COURS (SI ELLE N'A PAS ETE TERMINER)
            else if (reprendre)
            {
                pause = false;
                _screenManager.LoadScreen(_niveau1, new FadeTransition(GraphicsDevice, Color.Black));
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