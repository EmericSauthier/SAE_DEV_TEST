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

        private Pingouin _pingouin;
        private MouseState _mouseState;

        public bool clicMenu;
        public GameOver(Game1 game): base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            _pingouin = new Pingouin(150, 350);
            clicMenu = false;
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
            _pingouin.Perso.Play("dead");
            _pingouin.Perso.Update(deltaSeconds);
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_mouseState.X >= Champ.positionMessageMenu.X && _mouseState.Y >= Champ.positionMessageMenu.Y && _mouseState.X <= Champ.positionMessageMenu.X + Champ.messageMenu.Length * 24 && _mouseState.Y <= Champ.positionMessageMenu.Y + 24)
                {
                    clicMenu = true;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Black);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(Champ.police, $"{Champ.messagePerdu}", Champ.positionMessagePerdu, Color.Black);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{Champ.messageRejouer}", Champ.positionMessageRejouer, Color.Black);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{Champ.messageMenu}", Champ.positionMessageMenu, Color.Black);
            _myGame.SpriteBatch.End();
        }
    }
}
