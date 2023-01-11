using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Screens;

namespace Projet
{
    internal class ChoixNiveau : GameScreen
    {
        private Game1 _myGame;
        //deco
        private Texture2D _textureBackground;

        //SOURIS POUR GERER CLIC
        private MouseState _mouseState;

        //CHAMPS TEXTE BOUTON
        private string _niv1;
        private string _niv2;
        private string _messMenu;
        private Vector2 _positionNiv1;
        private Vector2 _positionNiv2;
        private Vector2 _positionMenu;

        public ChoixNiveau(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            _niv1 = "Niveau 1";
            _niv2 = "Niveau 2";
            _messMenu = "Retour au menu";
            _positionMenu = new Vector2(30, 50);
            _positionNiv1 = new Vector2(50, 150);
            _positionNiv2 = new Vector2(_positionNiv1.X+_niv2.Length*24+ 50, _positionNiv1.Y);

            base.Initialize();
        }
        public override void LoadContent()
        {
            _textureBackground= Content.Load<Texture2D>("Decors/bgChN");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                //si clic retour au menu
                if (_mouseState.X >= _positionMenu.X && _mouseState.Y >= _positionMenu.Y && _mouseState.X <= _positionMenu.X + _messMenu.Length * 24 && _mouseState.Y <= _positionMenu.Y + 24)
                {
                    _myGame.clicMenu = true;
                }
                //si clic niveau 1
                else if (_mouseState.X >= _positionNiv1.X && _mouseState.Y >= _positionNiv1.Y && _mouseState.X <= _positionNiv1.X + _niv1.Length * 24 && _mouseState.Y <= _positionNiv1.Y + 24)
                {
                    _myGame.goDesert = true;
                }//si clic niveau 2
                else if (_mouseState.X >= _positionNiv2.X && _mouseState.Y >= _positionNiv2.Y && _mouseState.X <= _positionNiv2.X + _niv2.Length * 24 && _mouseState.Y <= _positionNiv2.Y + 24)
                {
                    _myGame.goSnow = true;
                }


            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Gray);
            _myGame.SpriteBatch.Begin();
            //DECO
            _myGame.SpriteBatch.Draw(_textureBackground, new Vector2(0,0), Color.White);//LE FOND

            //TEXTE
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_niv1}", _positionNiv1, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_niv2}", _positionNiv2, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_messMenu}", _positionMenu, Color.White);
            _myGame.SpriteBatch.End();
        }
    }
}
