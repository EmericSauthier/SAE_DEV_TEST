﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System;
using System.Collections.Generic;

namespace Projet
{
    internal class Niveau1 : GameScreen
    {
        private Game1 _myGame;

        // Fenêtre
        public const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;
        private GraphicsDeviceManager _graphics;

        // GameManager
        private bool _gameOver;

        // Gestion des entrées
        private MouseState _mouseState;
        private KeyboardState _keyboardState;

        // Variables de map
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer _groundLayer;
        private TiledMapTileLayer _deadLayer;

        //JEU
        private Camera _camera;
        public static float scale;

        // Chrono
        private Vector2 _positionChrono;
        private float _chrono;
        private float _chronoDepFox1;
        private float _chronoDepEagle1;

        // ENTITE
        private Pingouin _pingouin;
        public int _largeurPingouin = 50, _hauteurPingouin = 40; // à déplacer ?
        private Rectangle _hitBoxPingouin;
        // Fox
        MonstreRampant[] _monstresRampants;
        MonstreRampant _fox1;
        // Eagle
        MonstreVolant _eagle1;

        // Piège
        Trap _ceilingTrap1;
        private float _chronoTrap1, _chronoInvincibility;
        public static bool _canCollidingTrap;

        //Recompense
        Recompenses []coins;
        public int largeurRecompense1 = 10, hauteurRecompense1 = 10;
        Song coinSound;

        // Life
        private Texture2D _heartSprite;
        private Vector2[] _heartsPositions;

        //Debug rectangle
        private Rectangle rFox;
        private Rectangle rTrap;
        private Rectangle rKillingFox;
        private Rectangle rRecompense;
        private Rectangle rEagle;
        private Rectangle rKillingEagle;

        //Portail
        private Vector2 _recoltePosition;
        private int _partiRecolleter;
        Recompenses[] partiPortail;
        Recompenses openingPortal;
        Recompenses closingPortal;
        Vector2[] _posiPartiPortail;

        public Niveau1(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _myGame.Window.Title = "Jeu du pingouin";

            // Etat de la partie
            _gameOver = false;

            // Camera
            scale = (float)0.5;
            _camera = new Camera();
            _camera.Initialize(_myGame.Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);

            // Chrono
            _chrono = 0;
            _chronoDepFox1 = 0;
            _chronoDepEagle1 = 0;
            _chronoInvincibility = 0;

            // Initialisation du pingouin et de sa position
            _pingouin = new Pingouin(LARGEUR_FENETRE / 2, 500 + (HAUTEUR_FENETRE / 2), scale);

            if (_myGame.reprendre)
            {
                _pingouin = new Pingouin(_myGame.dernierePosiPingouin.X, _myGame.dernierePosiPingouin.Y, scale);
                _myGame.reprendre = false;
            }
            else
            {
                _pingouin = new Pingouin(LARGEUR_FENETRE / 2, 500 + (HAUTEUR_FENETRE / 2), scale);
            }

            // Ennemis
            _fox1 = new MonstreRampant(new Vector2(1170, 850), "fox", 0.8, 12, 19*3, 14*3);

            _eagle1 = new MonstreVolant(new Vector2(200, 900), "eagle", 1, 12, 50, 30);

            // Traps
            _ceilingTrap1 = new Trap(new Vector2(1480, 800), 64/2, 64-20);

            //Recompenses
            coins = new Recompenses[4];
            int x = 1150;
            int y = 780;
            for (int i=0; i <4; i++)
            {
                coins[i] = new Recompenses(new Vector2(x+50*i, y), "piece", 0);
            }

            // Life
            _heartsPositions = new Vector2[3];

            //Portail
            _posiPartiPortail = new Vector2[] { new Vector2(1070, 642), new Vector2(2801, 300) };
            _partiRecolleter = 0;
            partiPortail = new Recompenses[_posiPartiPortail.Length];
            for (int i = 0; i < _posiPartiPortail.Length; i++)
            {
                partiPortail[i] = new Recompenses(_posiPartiPortail[i], "portal", 0);
            }
            openingPortal = new Recompenses(new Vector2(x, y), "portal", 1);
            closingPortal = new Recompenses(new Vector2(LARGEUR_FENETRE / 2-250, 500 + HAUTEUR_FENETRE / 2-50), "portal", 0);

            base.Initialize();
        }
        public override void LoadContent()
        {
            // Chargement de la map et du TileLayer du sol/décor
            _tiledMap = Content.Load<TiledMap>("Maps/snowmap1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _groundLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Ground");
            _deadLayer = _tiledMap.GetLayer<TiledMapTileLayer>("DeadZone");

            // Chargement du sprite du pingouin
            _pingouin.Perso = new AnimatedSprite(Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader()));

            // Chargement de la texture de la boule de neige
            _pingouin.SnowballTexture = this.Content.Load<Texture2D>("Perso/snowball");

            // Chargement du sprite du renard
            SpriteSheet foxSprite = Content.Load<SpriteSheet>("Ennemis_pieges/fox.sf", new JsonContentLoader());
            _fox1.LoadContent(foxSprite);

            // Chargement du sprite du piège
            SpriteSheet ceilingTrapSprite = Content.Load<SpriteSheet>("Ennemis_pieges/ceilingTrap.sf", new JsonContentLoader());
            _ceilingTrap1.LoadContent(ceilingTrapSprite);

            // Chargement du sprite de la recompense
            SpriteSheet spriteCoin = Content.Load<SpriteSheet>("Decors/spritCoin.sf", new JsonContentLoader());
            for (int i =0; i<4; i++)
            {
                coins[i].LoadContent(spriteCoin);
            }
            coinSound = Content.Load<Song>("Audio/coinSound");

            // Chargement de la texture des coeurs
            _heartSprite = Content.Load<Texture2D>("Life/heart");

            // Chargement texture eagle
            SpriteSheet eagleSprite = Content.Load<SpriteSheet>("Ennemis_pieges/eagle.sf", new JsonContentLoader());
            _eagle1.LoadContent(eagleSprite);

            //Chargement du sprite du portail
            SpriteSheet spritePortal = Content.Load<SpriteSheet>("Decors/portal.sf", new JsonContentLoader());
            for (int i=0; i < _posiPartiPortail.Length; i++)
            {
                partiPortail[i].LoadContent(spritePortal);
            }
            
            openingPortal.LoadContent(spritePortal);
            closingPortal.LoadContent(spritePortal);

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            // GameManager
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //CONDITION POUR GAGNER
            if (_partiRecolleter ==_posiPartiPortail.Length)
            {
                openingPortal.etat = 0;
            }

            //CONDITION POUR ALLER SUR LE MENU DU JEU
            if (_keyboardState.IsKeyDown(Keys.Tab))
            {
                _myGame.pause = !_myGame.pause;
            }
            else if (!_myGame.pause || _myGame.reprendre)
            {
                // Map
                _tiledMapRenderer.Update(gameTime);

                // Camera
                _camera.Update(gameTime, _pingouin);

                // Pingouin
                _myGame.dernierePosiPingouin = new Vector2(_pingouin.Position.GetHashCode()); //envoie dans game 1 la position du pingouin pour pouvoir reprendre a la meme position
                
                _pingouin.Update(_gameOver, deltaSeconds, _keyboardState, _groundLayer, _deadLayer);

                // Chrono
                _chrono += deltaSeconds;
                _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

                // Ennemis
                _chronoDepFox1 += deltaSeconds;
                _fox1.RightLeftMove(ref _chronoDepFox1);
                _fox1.Sprite.Update(deltaSeconds);

                _chronoDepEagle1 += deltaSeconds;
                _eagle1.IdleFlying(ref _chronoDepEagle1);
                _eagle1.Sprite.Update(deltaSeconds);

                // Recompense
                for (int i =0; i<4; i++)
                {
                    _chronoDepFox1 += deltaSeconds;
                    coins[i].Sprite.Play("coin");
                    coins[i].Sprite.Update(deltaSeconds);
                }

                //Compteur morceau de portail recolleter
                _recoltePosition = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2 + 50);

                //Portail
                openingPortal.Sprite.Play("openingPortal");
                openingPortal.Sprite.Play("closingPortal");
                openingPortal.Sprite.Update(deltaSeconds);
                closingPortal.Sprite.Update(deltaSeconds);
                for(int i=0; i < _posiPartiPortail.Length; i++)
                {
                    partiPortail[i].Sprite.Play("portal");
                    partiPortail[i].Sprite.Update(deltaSeconds);
                }

                // Traps
                _chronoTrap1 += deltaSeconds;
                _chronoInvincibility += deltaSeconds;
                _ceilingTrap1.PressActivation(ref _chronoTrap1, ref _canCollidingTrap);
                _ceilingTrap1.Sprite.Update(deltaSeconds);

                // Lifes
                for (int i = 0; i < _pingouin.MaxLife; i++)
                {
                    _heartsPositions[i] = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2 , _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);
                    _heartsPositions[i] += new Vector2(50 * i, 0);
                }

                // Collisions
                _hitBoxPingouin = new Rectangle((int)_pingouin.Position.X - 25, (int)_pingouin.Position.Y - 15, (int)(_largeurPingouin), (int)(_hauteurPingouin));

                if (Collision.IsCollidingTrap(_ceilingTrap1, _canCollidingTrap, ref rTrap, _hitBoxPingouin))
                {
                    _pingouin.TakeDamage(1, ref _chronoInvincibility);
                }
                // Collision du monstre avec le pingouin
                if (!_fox1.IsDied)
                {
                    if (Collision.IsCollidingMonstre(_pingouin, _fox1, ref rFox, ref rKillingFox, _hitBoxPingouin))
                    {
                        _pingouin.TakeDamage(1, ref _chronoInvincibility);
                    }
                }
                // Collision du monstre avec le pingouin
                if (!_eagle1.IsDied)
                {
                    if (Collision.IsCollidingMonstre(_pingouin, _eagle1, ref rEagle, ref rKillingEagle, _hitBoxPingouin))
                    {
                        _pingouin.TakeDamage(1, ref _chronoInvincibility);
                    }
                }


                for (int i =0; i<4; i++)
                {
                    if (coins[i].etat == 0)
                    {
                        //Collision de la recompense avec le pingouin
                        if (Collision.IsCollidingRecompense(coins[i], largeurRecompense1, hauteurRecompense1, ref rRecompense, _hitBoxPingouin))
                        {
                            if (_pingouin.CurrentLife == _pingouin.MaxLife)
                            {
                                double randomNb = new Random().NextDouble();
                                if(randomNb > 0.5)
                                {
                                    _pingouin.WalkVelocity *= 0.80;
                                }else
                                {
                                    _pingouin.WalkVelocity *= 1.20;
                                }
                                coins[i].etat = 1;
                                MediaPlayer.Play(coinSound);
                            }
                            else
                            {
                                coins[i].etat = 1;
                                _pingouin.Heal(1);
                                MediaPlayer.Play(coinSound);
                            }
                        }
                    }
                }

                // Mort
                if (_pingouin.CurrentLife <= 0)
                {
                    _myGame.clicDead = true;
                }
    }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Application du zoom de la caméra
            _tiledMapRenderer.Draw(_camera.OrthographicCamera.GetViewMatrix());
            // Affichage par rapport à la caméra
            _myGame.SpriteBatch.Begin(transformMatrix: _camera.OrthographicCamera.GetViewMatrix());

            // Affichage du pingouin
            _pingouin.Affiche(_myGame);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 40 * scale, _pingouin.Position.Y + 60 * scale, Color.Green, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X, _pingouin.Position.Y + 60 * scale, Color.Green, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 40 * scale, _pingouin.Position.Y + 60 * scale, Color.Green, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 40 * scale, _pingouin.Position.Y - 40 * scale, Color.Orange, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X, _pingouin.Position.Y - 40 * scale, Color.Orange, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 40 * scale, _pingouin.Position.Y - 40 * scale, Color.Orange, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y + 50 * scale, Color.Red, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y + 10 * scale, Color.Red, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y - 30 * scale, Color.Red, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y + 50 * scale, Color.Blue, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y + 10 * scale, Color.Blue, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y - 30 * scale, Color.Blue, 5);

            // Affichage du chrono
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono : {Chrono.AffichageChrono(_chrono)}", _positionChrono - new Vector2(20,0), Color.White);
            //_myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Trap : {Math.Round(_chronoTrap1, 2)}", _positionChrono + new Vector2(-100, 50), Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Invincibility : {Math.Round(_chronoInvincibility, 2)}", _positionChrono + new Vector2(-170, 100), Color.White);

            //Affichage du nombre de parti de portaill recuperer
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_partiRecolleter}" + $"/" + $"{_posiPartiPortail.Length}", _recoltePosition, Color.White);

            //Life
            for (int i = 0; i < _pingouin.CurrentLife; i++)
            {
                _myGame.SpriteBatch.Draw(_heartSprite, _heartsPositions[i], Color.White);
            }

            // Affichage des ennemis et des pièges
            if (!_fox1.IsDied)
            {
                _myGame.SpriteBatch.Draw(_fox1.Sprite, _fox1.Position, 0, new Vector2(3, 3));
            }
            //Trap
            _myGame.SpriteBatch.Draw(_ceilingTrap1.Sprite, _ceilingTrap1.Position, 0, new Vector2(1, 1));

            // Eagle
            if (!_eagle1.IsDied)
            {
                _myGame.SpriteBatch.Draw(_eagle1.Sprite, _eagle1.Position, 0, new Vector2(2, 2));
            }

            //Affichage des recompenses si elle n'as pas ete prise
            for (int i = 0; i<4; i++)
            {
                if (coins[i].etat == 0)
                {
                    _myGame.SpriteBatch.Draw(coins[i].Sprite, coins[i].Position, 0, new Vector2((float)0.15));
                }
            }

            //Affichage des parti du portail
            for(int i =0; i < _posiPartiPortail.Length; i++)
            {
                if (partiPortail[i].etat == 0)
                {
                    _myGame.SpriteBatch.Draw(partiPortail[i].Sprite, partiPortail[i].Position, 0, new Vector2((float)0.5));
                }
            }
            if (openingPortal.etat == 0)
            {
                _myGame.SpriteBatch.Draw(openingPortal.Sprite, openingPortal.Position, 0, new Vector2(2));
            }
            if (closingPortal.etat == 0)
            {
                _myGame.SpriteBatch.Draw(closingPortal.Sprite, closingPortal.Position, 0, new Vector2(2));
            }

            // Debug collision
            _myGame.SpriteBatch.DrawRectangle(_hitBoxPingouin, Color.Blue);
            _myGame.SpriteBatch.DrawRectangle(rTrap, Color.Orange);
            for (int i=0; i<4; i++)
            {
                if (coins[i].etat == 0)
                {
                    _myGame.SpriteBatch.DrawRectangle(rRecompense, Color.YellowGreen);
                }
            }
            
            if (!_fox1.IsDied)
            {
                _myGame.SpriteBatch.DrawRectangle(rFox, Color.Red);
                _myGame.SpriteBatch.DrawRectangle(rKillingFox, Color.DarkOrange);
            }
            if (!_eagle1.IsDied)
            {
                _myGame.SpriteBatch.DrawRectangle(rEagle, Color.Green);
                _myGame.SpriteBatch.DrawRectangle(rKillingEagle, Color.DarkGreen);
            }

            _myGame.SpriteBatch.End();
        }
    }
}
