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
        public Win(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            _pingouin = new Pingouin(150, 350);
            _myGame.clicMenu = false;
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
                if (_mouseState.X >= Champ.positionMessageMenu.X && _mouseState.Y >= Champ.positionMessageMenu.Y && _mouseState.X <= Champ.positionMessageMenu.X + Champ.messageMenu.Length * 24 && _mouseState.Y <= Champ.positionMessageMenu.Y + 24)
                {
                    _myGame.clicMenu = true;
                }
            }
            }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.White);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(Champ.police, $"{Champ.messageGagner}", Champ.positionMessageGagner, Color.Black);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{Champ.messageNivSuiv}", Champ.positionMessageNivSuiv, Color.Black);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{Champ.messageMenu}", Champ.positionMessageMenu, Color.Black);
            _myGame.SpriteBatch.End();
        }
    }
}
