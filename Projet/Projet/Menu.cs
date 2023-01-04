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
    internal class Menu : GameScreen
    {
        private Game1 _myGame;
        private Pingouin _pingouin;
        private KeyboardState _keyboardState;

        public Menu(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _pingouin = new Pingouin(20, 150);
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
            _keyboardState = Keyboard.GetState();

            _pingouin.Animate(false, _keyboardState);
            _pingouin.Perso.Update(deltaSeconds);
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Black);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.regle}", _myGame.positionRegle, Color.White);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.jouer}", _myGame.positionJouer, Color.White);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.niv}", _myGame.positionNiv, Color.White);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.quitter}", _myGame.positionQuitter, Color.White);
            //_myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position);
            _myGame.SpriteBatch.End();
        }
    }
}
