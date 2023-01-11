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

        //CHAMPS CONCERNANT LES TEXTES
        public static string messagePerdu;
        public static Vector2 positionMessagePerdu;
        public static string messageRejouer;
        public static string messageMenu;
        public static Vector2 positionMessageRejouer;
        public static Vector2 positionMessageMenu;

        //SOURIS
        private MouseState _mouseState;

        
        public GameOver(Game1 game): base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            //INITIALISATION TEXTE AFFICHER
            messagePerdu = "Game Over";
            messageRejouer = "Reessayer";
            messageMenu = "Menu";
            positionMessagePerdu = new Vector2(Game1.LARGEUR_FENETRE/2-messagePerdu.Length*25, Game1.HAUTEUR_FENETRE/2-24);
            positionMessageRejouer = new Vector2(Game1.LARGEUR_FENETRE/4, positionMessagePerdu.Y+150);
            positionMessageMenu = new Vector2(Game1.LARGEUR_FENETRE/2+ messagePerdu.Length * 24, positionMessagePerdu.Y+150);

            //MISE EN PLACE DE PINGOUIN DECO
            _pingouinTab = new Pingouin[6];
            for (int i =0; i < 6; i++)
            {
                _pingouinTab[i] = new Pingouin(50+100*i, 600);
            }
            
            _pingouin = new Pingouin(70, 90);

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
                if (_mouseState.X >= positionMessageMenu.X && _mouseState.Y >= positionMessageMenu.Y && _mouseState.X <= positionMessageMenu.X + 4 * 24 && _mouseState.Y <= positionMessageMenu.Y + 24)
                {
                    _myGame.clicMenu = true;
                }
                //CONDITION RETOURNE PARTIE
                else if (_mouseState.X >= positionMessageRejouer.X && _mouseState.Y >= positionMessageRejouer.Y && _mouseState.X <= positionMessageRejouer.X + messageRejouer.Length * 24 && _mouseState.Y <= positionMessageRejouer.Y + 24)
                {
                    if (_myGame.nivActu == 1)
                        _myGame.goDesert = true;
                    else if (_myGame.nivActu == 2)
                        _myGame.goSnow = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Black);
            _myGame.SpriteBatch.Begin();
            //TEXTE
            _myGame.SpriteBatch.DrawString(_policeGO, $"{messagePerdu}", positionMessagePerdu, Color.Red);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{messageRejouer}", positionMessageRejouer, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{messageMenu}", positionMessageMenu, Color.White);

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
