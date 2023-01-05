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
    internal class Win : GameScreen
    {
        private Game1 _myGame;

        private Pingouin _pingouin;
        private MouseState _mouseState;

        //CHAMPS CONCERNANT LES TEXTES
        public static string messageGagner;
        public static Vector2 positionMessageGagner;
        public static string messageNivSuiv;
        public static Vector2 positionMessageNivSuiv;
        public static string messageMenu;
        public static Vector2 positionMessageMenu;

        public Win(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            _pingouin = new Pingouin(150, 350);
            _myGame.clicMenu = false;

            //INITIALISATION TEXTE AFFICHER
            messageGagner = "Niveau completer !";
            messageMenu = "Menu";
            messageNivSuiv = "Niveau suivant -->";
            positionMessageGagner = new Vector2(50, 50);
            positionMessageMenu = new Vector2(50, 350);
            positionMessageNivSuiv = new Vector2(250, 350);
            base.Initialize();
        }
        public override void LoadContent()
        {
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            _pingouin.Perso = new AnimatedSprite(spriteSheet);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pingouin.Perso.Play("celebrate");
            _pingouin.Perso.Update(deltaSeconds);
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_mouseState.X >= positionMessageMenu.X && _mouseState.Y >= positionMessageMenu.Y && _mouseState.X <= positionMessageMenu.X + messageMenu.Length * 24 && _mouseState.Y <= positionMessageMenu.Y + 24)
                {
                    _myGame.clicMenu = true;
                }
            }
            }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.White);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(Game1.police, $"{messageGagner}", positionMessageGagner, Color.Black);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{messageNivSuiv}", positionMessageNivSuiv, Color.Black);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{messageMenu}", positionMessageMenu, Color.Black);
            _myGame.SpriteBatch.End();
        }
    }
}
