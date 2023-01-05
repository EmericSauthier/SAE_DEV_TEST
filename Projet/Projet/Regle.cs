using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    internal class Regle : GameScreen
    {
        private Game1 _myGame;

        //PINGOUIN DECO
        private Pingouin _pingouinSauter;
        private Pingouin _pingouinGlisser;
        private Pingouin _pingouinAvancer;

        //SOURIS POUR GERER CLIC
        private MouseState _mouseState;

        //CHAMPS TEXTE
        private string _menuTXT;
        private string _texteEnnemi;
        private string _texteControle;
        private string _texteIntroEnnemi;
        private Vector2 _positionMenu;
        private Vector2 _positiontxtEnnemi;
        private Vector2 _positiontxtControle;
        private Vector2 _positiontxtIntroEnnemi;
            
            //EXPLI DEPLACEMENT
        private string _sauter;
        private string _glisser;
        private string _avancer;
        private string _attaquer;
        private string _relever;
        private Vector2 _positionSauter;
        private Vector2 _positionGlisser;
        private Vector2 _positionAvancer;
        private Vector2 _positionAttaquer;
        private Vector2 _positionRelever;

        public Regle(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            //INITIALISATION DES TEXTES
            _menuTXT = "Menu";
                //DEPLACEMENT
            _texteControle = "Deplacement :";
            _avancer = "Maintener la fleche droite pour avancer.";
            _sauter = "Sauter avec la touche espace !";
            _glisser = "Glisser grace a la fleche du bas.";
            _relever = "Relever vous avec la fleche du haut.";
            _attaquer = "Attaquer avec ...";
                //ENNEMI
            _texteEnnemi = "Attention aux predateurs !";
            _texteIntroEnnemi = "Durant votre parcours, faites attention aux ennemis sur votre \nchemin, il pourrait bien vous devorer ! Eviter les ou attaquer !";
            

            //POSITION
            _positionMenu = new Vector2(30, 50);

            _positiontxtControle = new Vector2(50, 150);
            _positionAvancer = new Vector2(_positionMenu.X, _positiontxtControle.Y + 70);
            _positionSauter = new Vector2(_positionMenu.X+100, _positionAvancer.Y + 80);
            _positionGlisser = new Vector2(_positionMenu.X, _positionSauter.Y + 80);
            _positionRelever = new Vector2(_positionMenu.X, _positionGlisser.Y + 30);
            _positionAttaquer = new Vector2(_positionMenu.X, _positionRelever.Y + 80);

            _positiontxtEnnemi = new Vector2(_positiontxtControle.X, _positionAttaquer.Y+60);
            _positiontxtIntroEnnemi = new Vector2(_positionMenu.X, _positiontxtEnnemi.Y+35);


            //CREATION DECO
            _pingouinSauter = new Pingouin(_positionSauter.X-80, _positionSauter.Y);
            _pingouinAvancer = new Pingouin(_positionAvancer.X+_avancer.Length*16, _positionAvancer.Y);
            _pingouinGlisser = new Pingouin(_positionGlisser.X+_glisser.Length*18, _positionGlisser.Y);

            base.Initialize();
        }
        public override void LoadContent()
        {
            //PINGOUIN DECO
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            _pingouinSauter.Perso = new AnimatedSprite(spriteSheet);
            _pingouinGlisser.Perso = new AnimatedSprite(spriteSheet);
            _pingouinAvancer.Perso = new AnimatedSprite(spriteSheet);

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            //ANIMATION DU PINGOUIN DECO
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pingouinSauter.Perso.Play("jump");
            _pingouinAvancer.Perso.Play("walkForward");
            _pingouinGlisser.Perso.Play("beforeSlide");
            _pingouinSauter.Perso.Update(deltaSeconds);
            _pingouinAvancer.Perso.Update(deltaSeconds);
            _pingouinGlisser.Perso.Update(deltaSeconds);

            //GESTION CLIC
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                //si clic retour au menu
                if (_mouseState.X >= _positionMenu.X && _mouseState.Y >= _positionMenu.Y && _mouseState.X <= _positionMenu.X + _menuTXT.Length * 24 && _mouseState.Y <= _positionMenu.Y + 24)
                {
                    _myGame.clicMenu = true;
                }


            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Gray);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_menuTXT}", _positionMenu, Color.White);

            _myGame.SpriteBatch.DrawString(Game1.police, $"{_texteControle}", _positiontxtControle, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_sauter}", _positionSauter, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_glisser}", _positionGlisser, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_avancer}", _positionAvancer, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_relever}", _positionRelever, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_attaquer}", _positionAttaquer, Color.White);
            //PINGOUIN
            _myGame.SpriteBatch.Draw(_pingouinSauter.Perso, _pingouinSauter.Position);
            _myGame.SpriteBatch.Draw(_pingouinAvancer.Perso, _pingouinAvancer.Position);
            _myGame.SpriteBatch.Draw(_pingouinGlisser.Perso, _pingouinGlisser.Position);

            _myGame.SpriteBatch.DrawString(Game1.police, $"{_texteEnnemi}", _positiontxtEnnemi, Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_texteIntroEnnemi}", _positiontxtIntroEnnemi, Color.White);
            _myGame.SpriteBatch.End();
        }
    }
}
