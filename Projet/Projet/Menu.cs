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
        private MouseState _mouseState;

        public bool clicChoixNiv;
        public Menu(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _pingouin = new Pingouin(150, 350);
            clicChoixNiv = false;
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
            


            //gestion souris
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_mouseState.X >= _myGame.positionQuitter.X && _mouseState.Y >= _myGame.positionQuitter.Y && _mouseState.X <= _myGame.positionQuitter.X + _myGame.quitter.Length*24 && _mouseState.Y <= _myGame.positionQuitter.Y + 24)
                { }//Exit();
                //envoie à la scene des regles
                else if (_mouseState.X >= _myGame.positionRegle.X && _mouseState.Y >= _myGame.positionRegle.Y && _mouseState.X <= _myGame.positionRegle.X + _myGame.regle.Length * 24 && _mouseState.Y <= _myGame.positionRegle.Y + 24)
                {
                    
                }
                //envoie a la scene de jeu
                else if (_mouseState.X >= _myGame.positionJouer.X && _mouseState.Y >= _myGame.positionJouer.Y && _mouseState.X <= _myGame.positionJouer.X + _myGame.jouer.Length * 24 && _mouseState.Y <= _myGame.positionJouer.Y + 24)
                {

                }
                //envoie a la scene de choix de niveau
                else if (_mouseState.X >= _myGame.positionNiv.X && _mouseState.Y >= _myGame.positionNiv.Y && _mouseState.X <= _myGame.positionNiv.X + _myGame.niv.Length * 24 && _mouseState.Y <= _myGame.positionNiv.Y + 24)
                {
                    clicChoixNiv = true;
                }

            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Black);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.regle}", _myGame.positionRegle, Color.White);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.jouer}", _myGame.positionJouer, Color.White);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.niv}", _myGame.positionNiv, Color.White);
            _myGame.SpriteBatch.DrawString(_myGame.police, $"{_myGame.quitter}", _myGame.positionQuitter, Color.White);
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position);
            _myGame.SpriteBatch.End();
        }
    }
}
