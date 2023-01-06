using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
    internal class Parametres : GameScreen
    {
        private Game1 _myGame;

        private Vector2 _positionGauche;
        private Vector2 _positionDroite;
        private Vector2 _positionSauter;
        private Vector2 _positionGlisser;

        private MouseState _mouseState;
        private KeyboardState _keyboardState;

        public Parametres(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _positionGauche = new Vector2(250, 100);
            _positionDroite = _positionGauche + new Vector2(0, 150);
            _positionSauter = _positionDroite + new Vector2(0, 150);
            _positionGlisser = _positionSauter + new Vector2(0, 150);

            base.Initialize();
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_mouseState.X >= _positionGauche.X && _mouseState.X <= _positionGauche.X+120 && _mouseState.Y >= _positionGauche.Y && _mouseState.Y <= _positionGauche.Y + 50)
                {
                    if (_keyboardState.GetPressedKeyCount() == 1 && _keyboardState.GetPressedKeys()[0] != Keys.Escape)
                    {
                        System.Diagnostics.Debug.WriteLine(_keyboardState.GetPressedKeys()[0]);
                    }
                }
            }
        }
        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.White);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(Game1.police, "Gauche", _positionGauche, Color.Wheat);
            _myGame.SpriteBatch.DrawRectangle(_positionGauche, new Size2(120, 50), Color.Black, 5);
            _myGame.SpriteBatch.End();
        }
    }
}
