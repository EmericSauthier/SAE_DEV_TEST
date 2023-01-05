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

        //PINGOUIN DECO
        private Pingouin _pingouin;
        private Texture2D[] _textureFlocons;
        private Texture2D _textureFond;
        private Vector2[] _positionFlocons;
        private Vector2 _positionFond;

        //CHAMPS CONCERNANT LES TEXTES
        public static string regle;
        public static Vector2 positionRegle;
        public static string jouer;
        public static Vector2 positionJouer;
        public static string niv;
        public static Vector2 positionNiv;
        public static string quitter;
        public static Vector2 positionQuitter;


        //GESTION SOURIS
        private MouseState _mouseState;
        public bool clicChoixNiv;


        public Menu(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            //CREATION PINGOUIN & DECO
            _pingouin = new Pingouin(150, 350);
            Random random = new Random();
            _positionFlocons = new Vector2[20];
            for (int i = 0; i < 20; i++)
            {
                _positionFlocons[i] = new Vector2(random.Next(800), 0);
            }
            _positionFond = new Vector2(0, 0);

            //GESTION CLIC
            clicChoixNiv = false;

            //INITIALISATION TEXTE AFFICHER
            regle = "Notes de pingouin";
            jouer = "Jouer";
            niv = "Charger un niveau";
            quitter = "Quitter";

            positionRegle = new Vector2(300+regle.Length*12, 200);
            positionJouer = new Vector2(positionRegle.X, positionRegle.Y+100);
            positionNiv = new Vector2(positionRegle.X, positionJouer.Y+100);
            positionQuitter = new Vector2(positionRegle.X, positionNiv.Y+100);
            base.Initialize();
        }
        public override void LoadContent()
        {
            //PINGOUIN & DECO
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            _pingouin.Perso = new AnimatedSprite(spriteSheet);
            _textureFlocons = new Texture2D[20];
            for (int i = 0; i < 20; i++)
                _textureFlocons[i] = Content.Load<Texture2D>("flocon");
            _textureFond = Content.Load<Texture2D>("fondMenu");


            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            //ANIMATION DU PINGOUIN
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pingouin.Perso.Play("celebrate");
            _pingouin.Perso.Update(deltaSeconds);

            //DECO
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                _positionFlocons[i].Y += 1 * 5 * deltaTime;
                if (_positionFlocons[i].Y >= 1000)
                {
                    _positionFlocons[i] = new Vector2(random.Next(800), 0);
                }
            }
            



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
                    _myGame.clicRegle = true;
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
            _myGame.GraphicsDevice.Clear(Color.White);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.Draw(_textureFond, _positionFond, Color.White);//LE FOND
            _myGame.SpriteBatch.DrawString(Game1.police, $"{regle}", positionRegle, Color.Black);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{jouer}", positionJouer, Color.Black);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{niv}", positionNiv, Color.Black);
            _myGame.SpriteBatch.DrawString(Game1.police, $"{quitter}", positionQuitter, Color.Black);
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position);//LE PINGOUIN
            
            /*for (int i=0; i<20; i++)
            {
                _myGame.SpriteBatch.Draw(_textureFlocons[i], _positionFlocons[i], 0, Vector2((float)0.05,(float)0.05));
            }*/
            _myGame.SpriteBatch.End();
        }
    }
}
