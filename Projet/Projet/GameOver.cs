﻿using Microsoft.Xna.Framework;
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

        //DECORATION PNGOUIN
        private Pingouin _pingouin;
        private Pingouin[] _pingouinTab;

        //CHAMPS CONCERNANT LES TEXTES
        public static string messagePerdu;
        public static Vector2 positionMessagePerdu;
        public static string messageRejouer;
        public static Vector2 positionMessageRejouer;

        //SOURIS
        private MouseState _mouseState;

        
        public GameOver(Game1 game): base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            //TEXTE AFFICHER
            messagePerdu = "C'est mort...";
            messageRejouer = "Reessayer";
            positionMessagePerdu = new Vector2(50, 50);
            positionMessageRejouer = new Vector2(250, 350);

            //MISE EN PLACE DE PINGOUIN DECO
            _pingouinTab = new Pingouin[6];
            for (int i =0; i < 6; i++)
            {
                _pingouinTab[i] = new Pingouin(50+100*i, 600);
            }
            
            _pingouin = new Pingouin(350, 50);
            _myGame.clicMenu = false;
            base.Initialize();
        }
        public override void LoadContent()
        {
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            for (int i=0; i<6; i++)
            {
                _pingouinTab[i].Perso = new AnimatedSprite(spriteSheet);
            }
            
            
            _pingouin.Perso = new AnimatedSprite(spriteSheet);
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i =0; i <6; i++)
            {
                _pingouinTab[i].Perso.Play("dead");
                _pingouinTab[i].Perso.Update(deltaSeconds);
            }

            _pingouin.Perso.Play("dead");
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
            _myGame.GraphicsDevice.Clear(Color.Black);
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.DrawString(Champ.police, $"{messagePerdu}", positionMessagePerdu, Color.White);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{messageRejouer}", positionMessageRejouer, Color.White);
            _myGame.SpriteBatch.DrawString(Champ.police, $"{Champ.messageMenu}", Champ.positionMessageMenu, Color.White);
            for (int i=0; i<6; i++)
            {
                _myGame.SpriteBatch.Draw(_pingouinTab[i].Perso, _pingouinTab[i].Position);
            }
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position);
            _myGame.SpriteBatch.End();
        }
    }
}
