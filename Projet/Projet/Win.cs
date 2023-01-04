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

        public Win(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void LoadContent()
        {
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.White);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.messageGagner}", _myGame.positionMessageGagner, Color.Black);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.messageNivSuiv}", _myGame.positionMessageNivSuiv, Color.Black);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.messageMenu}", _myGame.positionMessageMenu, Color.Black);
            _myGame.SpriteBatch.End();
        }
    }
}
