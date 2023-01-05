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
using System;

namespace Projet
{
    internal class Menu : GameScreen
    {
        private Game1 _myGame;
        private Pingouin _pingouin;
        private MouseState _mouseState;

        //CHAMPS CONCERNANT LES TEXTES
        public static string regle;
        public static Vector2 positionRegle;
        public static string jouer;
        public static Vector2 positionJouer;
        public static string niv;
        public static Vector2 positionNiv;
        public static string quitter;
        public static Vector2 positionQuitter;

        public bool clicChoixNiv;
        public Menu(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            _pingouin = new Pingouin(150, 350);
            clicChoixNiv = false;

            //INITIALISATION TEXTE AFFICHER
            regle = "Notes de pingouin";
            jouer = "Jouer";
            niv = "Charger un niveau";
            quitter = "Quitter";
            float tailleRegle = 24 * regle.Length;
            positionRegle = new Vector2(tailleRegle / 2, 50);
            positionJouer = new Vector2(tailleRegle / 2, 150);
            positionNiv = new Vector2(tailleRegle / 2, 250);
            positionNiv = new Vector2(tailleRegle / 2, 250);
            positionQuitter = new Vector2(tailleRegle / 2, 350);
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
                if (_mouseState.X >= positionQuitter.X && _mouseState.Y >= positionQuitter.Y && _mouseState.X <= positionQuitter.X + quitter.Length*24 && _mouseState.Y <= positionQuitter.Y + 24)
                {
                    _myGame.clicDead = true;
                    _myGame.clicArret = true;
                }
                //envoie à la scene des regles
                else if (_mouseState.X >= positionRegle.X && _mouseState.Y >= positionRegle.Y && _mouseState.X <= positionRegle.X + regle.Length * 24 && _mouseState.Y <= positionRegle.Y + 24)
                {
                    
                }
                //envoie a la scene de jeu
                else if (_mouseState.X >= positionJouer.X && _mouseState.Y >= positionJouer.Y && _mouseState.X <= positionJouer.X + jouer.Length * 24 && _mouseState.Y <= positionJouer.Y + 24)
                {

                }
                //envoie a la scene de choix de niveau
                else if (_mouseState.X >= positionNiv.X && _mouseState.Y >= positionNiv.Y && _mouseState.X <= positionNiv.X + niv.Length * 24 && _mouseState.Y <= positionNiv.Y + 24)
                {
                    clicChoixNiv = true;
                }

            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Black);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(Champ.police, $"{regle}", positionRegle, Color.White);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{jouer}", positionJouer, Color.White);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{niv}", positionNiv, Color.White);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{quitter}", positionQuitter, Color.White);
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position);
            _myGame.SpriteBatch.End();
        }
    }
}
