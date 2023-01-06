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
    internal class GameOver: GameScreen
    {
        private Game1 _myGame;

        //police
        private static SpriteFont _policeGO;
        //DECORATION PNGOUIN
        private Pingouin _pingouin;
        private Pingouin[] _pingouinTab;
        private Texture2D _textureFond;
        private Vector2 _positionFond;

        //CHAMPS CONCERNANT LES TEXTES
        public static string messagePerdu;
        public static Vector2 positionMessagePerdu;
        public static string messageRejouer;
        public static Vector2 positionMessageRejouer;

        //SOURIS
        private MouseState _mouseState;

        
        public GameOver(Game1 game): base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            //INITIALISATION TEXTE AFFICHER
            messagePerdu = "C'est mort...";
            messageRejouer = "Reessayer";
            positionMessagePerdu = new Vector2(50, 50);
            positionMessageRejouer = new Vector2(250, 350);

            //MISE EN PLACE DE PINGOUIN DECO
            _pingouinTab = new Pingouin[6];
            for (int i =0; i < 6; i++)
            {
                _pingouinTab[i] = new Pingouin(50+100*i, 600);
            }
            
            _pingouin = new Pingouin(350, 50);
            _positionFond = new Vector2(0, 0);

            //GESTION CLIC
            _myGame.clicMenu = false;
            base.Initialize();
        }
        public override void LoadContent()
        {
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            for (int i=0; i<6; i++)
            {
                _pingouinTab[i].Perso = new AnimatedSprite(spriteSheet);
            }
            _pingouin.Perso = new AnimatedSprite(spriteSheet);

            _textureFond = Content.Load<Texture2D>("Decor/fondGameOver");

            //POLICE
            _policeGO = Content.Load<SpriteFont>("Font/FontGameOver");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i =0; i <6; i++)
            {
                _pingouinTab[i].Perso.Play("dead");
                _pingouinTab[i].Perso.Update(deltaSeconds);
            }

            _pingouin.Perso.Play("dead");
            _pingouin.Perso.Update(deltaSeconds);
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                //CONDITION RETOUR MENU
                if (_mouseState.X >= Win.positionMessageMenu.X && _mouseState.Y >= Win.positionMessageMenu.Y && _mouseState.X <= Win.positionMessageMenu.X + Win.messageMenu.Length * 24 && _mouseState.Y <= Win.positionMessageMenu.Y + 24)
                {
                    _myGame.clicMenu = true;
                }
                //CONDITION RETOURNE PARTIE
                else if (_mouseState.X >= positionMessageRejouer.X && _mouseState.Y >= positionMessageRejouer.Y && _mouseState.X <= positionMessageRejouer.X + messageRejouer.Length * 24 && _mouseState.Y <= positionMessageRejouer.Y + 24)
                {
                    _myGame.clicNiveau1 = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Black);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.Draw(_textureFond, _positionFond, Color.White);//LE FOND
            //TEXTE
            _myGame.SpriteBatch.DrawString(_policeGO, $"{messagePerdu}", positionMessagePerdu, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{messageRejouer}", positionMessageRejouer, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{Win.messageMenu}", Win.positionMessageMenu, Color.White);

            //PINGOUIN
            //for (int i=0; i<6; i++)
            //{
            //    _myGame.SpriteBatch.Draw(_pingouinTab[i].Perso, _pingouinTab[i].Position);
            //}
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position);
            _myGame.SpriteBatch.End();
        }
    }
}
