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
    internal class Snow : GameScreen
    {
        private Game1 _myGame;

        //Activation cheat
        bool cheat;
        
        // Fenêtre
        public const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;
        private GraphicsDeviceManager _graphics;

        // GameManager
        private bool _gameOver;
        private GameManager _manager;

        // Gestion des entrées
        private MouseState _mouseState;
        private KeyboardState _keyboardState;

        // Variables de map
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer _spikesLayer;

        // JEU
        private Camera _camera;
        public static float scale;

        // Chrono
        private Vector2 _positionChrono;

        // ENTITE
        List<MonstreRampant> monstresRampants;
        List<MonstreVolant> monstresVolants;
        List<Trap> traps;
        private Pingouin _pingouin;
        private Rectangle _hitBoxPingouin;

        // Recompense
        Recompenses []coins;
        Vector2[] _posiCoins;
        public int largeurRecompense1 = 10, hauteurRecompense1 = 10;

        // Life
        private Texture2D _heartSprite;
        private Vector2[] _heartsPositions;

        //Portail
        private Vector2 _recoltePosition;
        private int _partiRecolleter;
        Recompenses[] partiPortail;
        Recompenses openingPortal;
        Recompenses closingPortal;
        Vector2[] _posiPartiPortail;

        // Audio
        Song recupAllPortalSound;
        Song coinSound;
        Song monsterTouchPingouin;
        Song trapTouchPingouin;

        // Tableau de boule de neige
        private Snowball[] _snowballs;
        private Texture2D _snowballTexture;

        public Snow(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _myGame.Window.Title = "Jeu du pingouin";

            // Etat de la partie
            _gameOver = false;
            _manager = new GameManager();

            // Camera
            scale = (float)0.5;
            _camera = new Camera();
            _camera.Initialize(_myGame.Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);

            Chrono.InitializeChronos();

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

            // Tableau monstre rampant
            monstresRampants = new List<MonstreRampant>();
            monstresRampants.Add(new MonstreRampant(new Vector2(1170, 850), "fox", 0.5, 5));
            monstresRampants.Add(new MonstreRampant(new Vector2(2430, 725), "fox", 0.4, 5));
            monstresRampants.Add(new MonstreRampant(new Vector2(3660, 500), "fox", 0.4, 5));
            monstresRampants.Add(new MonstreRampant(new Vector2(3690, 245), "fox", 0.4, 5));
            // Tableau monstre volant
            monstresVolants = new List<MonstreVolant>();
            monstresVolants.Add(new MonstreVolant(new Vector2(1000, 500), "eagle", 1, 12));
            monstresVolants.Add(new MonstreVolant(new Vector2(2500, 100), "eagle", 1, 12));
            // Tableau Traps
            traps = new List<Trap>();
            traps.Add(new Trap(new Vector2(2728, 897), "press"));
            traps.Add(new Trap(new Vector2(1618, 767), "press"));
            traps.Add(new Trap(new Vector2(3200, 897), "press"));
            traps.Add(new Trap(new Vector2(4300, 735), "press"));

            // Recompenses
            _posiCoins = new Vector2[] { new Vector2(986, 1122), new Vector2(1000, 1122)};
            coins = new Recompenses[_posiCoins.Length];
            for (int i=0; i <_posiCoins.Length; i++)
            {
                coins[i] = new Recompenses(_posiCoins[i], "piece", 0);
            }

            // Life
            _heartsPositions = new Vector2[3];

            // Portail
            _posiPartiPortail = new Vector2[] { new Vector2(1070, 642), new Vector2(2801, 300) };
            _partiRecolleter = 0;
            partiPortail = new Recompenses[_posiPartiPortail.Length];
            for (int i = 0; i < _posiPartiPortail.Length; i++)
            {
                partiPortail[i] = new Recompenses(_posiPartiPortail[i], "portal", 0);
            }
            openingPortal = new Recompenses(new Vector2(6246, 740), "portal", 1);
            closingPortal = new Recompenses(new Vector2(364,900), "portal", 0);

            _snowballs = new Snowball[0];

            base.Initialize();
        }
        public override void LoadContent()
        {
            // Chargement de la map et du TileLayer du sol/décor
            _tiledMap = Content.Load<TiledMap>("Maps/snowmap1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _spikesLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Spikes");

            // Chargement du sprite du pingouin
            _pingouin.Perso = new AnimatedSprite(Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader()));

            // Chargement de la texture de la boule de neige
            _manager.SnowballTexture = Content.Load<Texture2D>("Perso/snowball");

            // Chargement du sprite du renard
            SpriteSheet foxSprite = Content.Load<SpriteSheet>("Ennemis_pieges/fox.sf", new JsonContentLoader());
            for (int i = 0; i < monstresRampants.Count; i++)
            {
                monstresRampants[i].LoadContent(foxSprite);
            };

            // Chargement texture eagle
            SpriteSheet eagleSprite = Content.Load<SpriteSheet>("Ennemis_pieges/eagle.sf", new JsonContentLoader());
            for (int i = 0; i < monstresVolants.Count; i++)
            {
                monstresVolants[i].LoadContent(eagleSprite);
            }

            // Chargement du sprite du piège
            SpriteSheet ceilingTrapSprite = Content.Load<SpriteSheet>("Ennemis_pieges/ceilingTrap.sf", new JsonContentLoader());
            for (int i = 0; i < traps.Count; i++)
            {
                traps[i].LoadContent(ceilingTrapSprite);
            }

            // Chargement du sprite de la recompense
            SpriteSheet spriteCoin = Content.Load<SpriteSheet>("Decors/spritCoin.sf", new JsonContentLoader());
            for (int i =0; i< _posiCoins.Length; i++)
            {
                coins[i].LoadContent(spriteCoin);
            }

            // Chargement de la texture des coeurs
            _heartSprite = Content.Load<Texture2D>("Life/heart");

            // Chargement du sprite du portail
            SpriteSheet spritePortal = Content.Load<SpriteSheet>("Decors/portal.sf", new JsonContentLoader());
            for (int i=0; i < _posiPartiPortail.Length; i++)
            {
                partiPortail[i].LoadContent(spritePortal);
            }
            openingPortal.LoadContent(spritePortal);
            closingPortal.LoadContent(spritePortal);


            // Chargement des audio
            coinSound = Content.Load<Song>("Audio/coinSound");
            recupAllPortalSound = Content.Load<Song>("Audio/recupAllPortal");
            monsterTouchPingouin = Content.Load<Song>("Audio/monsterTouchPingouin");
            trapTouchPingouin = Content.Load<Song>("Audio/trapTouchPingouin");


            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {

            // GameManager
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // CONDITION POUR GAGNER
            if (_partiRecolleter ==_posiPartiPortail.Length)
            {
                openingPortal.etat = 0;
                MediaPlayer.Play(recupAllPortalSound);
            }

            // CONDITION POUR ALLER SUR LE MENU DU JEU
            if (_keyboardState.IsKeyDown(Keys.Tab))
            {
                _myGame.pause = !_myGame.pause;
            }
            else if (!_myGame.pause || _myGame.reprendre)
            {
                //Code cheat
                if (_keyboardState.IsKeyDown(Keys.F2))
                    cheat = true;
                if (cheat)
                {
                    if (_keyboardState.IsKeyDown(Keys.C))
                    {
                        _partiRecolleter = _posiPartiPortail.Length; //L'entiereté des parti de portail est récolleter
                        for (int i = 0; i < _posiPartiPortail.Length; i++)
                        {
                            partiPortail[i].etat = 0;
                        }
                    }
                    if (_keyboardState.IsKeyDown(Keys.Insert))
                    {
                        _pingouin.Position = new Vector2(500, 900); //Le pingouin est tp a son point de départ
                    }
                    if (_keyboardState.IsKeyDown(Keys.V))
                    {
                        _pingouin.CurrentLife = 3;//Le pingouin récupere toute sa vie
                    }
                    if (_keyboardState.IsKeyDown(Keys.End))
                    {
                        _pingouin.Position = new Vector2(6238, 740); //Le pingouin est tp a la zone de fin
                    }
                    if (_keyboardState.IsKeyDown(Keys.F))
                    {
                        _pingouin.Position += new Vector2(0, -5);
                    }
                    if (_keyboardState.IsKeyDown(Keys.P))
                    {
                        _pingouin.WalkVelocity = 2;
                    }
                }
                    

                // Map
                _tiledMapRenderer.Update(gameTime);

                // Camera
                _camera.Update(gameTime, _pingouin);

                // Pingouin
                _myGame.dernierePosiPingouin = new Vector2(_pingouin.Position.GetHashCode()); // envoie dans game 1 la position du pingouin pour pouvoir reprendre a la meme position

                _manager.Update(_myGame, _keyboardState, _pingouin, ref _snowballs, _tiledMap, deltaSeconds, _spikesLayer);

                // Chrono
                Chrono.UpdateChronos(deltaSeconds);
                _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

                // Ennemis
                // Rampants
                for (int i = 0; i < monstresRampants.Count; i++)
                {
                    monstresRampants[i].RightLeftMove(gameTime);
                    monstresRampants[i].Sprite.Update(deltaSeconds);
                    monstresRampants[i].UpdateBoxes();
                }

                // Volants
                for (int i = 0; i < monstresVolants.Count; i++)
                {
                    monstresVolants[i].Move(gameTime, _pingouin);
                    monstresVolants[i].Sprite.Update(deltaSeconds);
                    monstresVolants[i].UpdateBoxes();
                }

                // Recompense
                for (int i =0; i< _posiCoins.Length; i++)
                {
                    coins[i].Sprite.Play("coin");
                    coins[i].Sprite.Update(deltaSeconds);
                }

                // Compteur morceau de portail recolleter
                _recoltePosition = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2 + 50);

                // Portail
                openingPortal.Sprite.Play("openingPortal");
                closingPortal.Sprite.Play("closingPortal");
                openingPortal.Sprite.Update(deltaSeconds);
                closingPortal.Sprite.Update(deltaSeconds);
                for(int i=0; i < _posiPartiPortail.Length; i++)
                {
                    partiPortail[i].Sprite.Play("portal");
                    partiPortail[i].Sprite.Update(deltaSeconds);
                }

                // Traps
                for (int i = 0; i < traps.Count; i++)
                {
                    traps[i].PressActivation(gameTime);
                    traps[i].Sprite.Update(deltaSeconds);
                    traps[i].UpdateBoxes();
                }

                // Lifes
                for (int i = 0; i < _pingouin.MaxLife; i++)
                {
                    _heartsPositions[i] = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2 , _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);
                    _heartsPositions[i] += new Vector2(50 * i, 0);
                }

                
                // Collisions des traps avec le pingouin
                for (int i = 0; i < traps.Count; i++)
                {
                    if (Collision.IsCollidingTrap(traps[i], _pingouin.HitBox))
                    {
                        _pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                        MediaPlayer.Play(trapTouchPingouin);
                    }
                }

                // Collision des rampants avec le pingouin
                for (int i = 0; i < monstresRampants.Count; i++)
                {
                    if (!monstresRampants[i].IsDied)
                    {
                        if (Collision.IsCollidingMonstre(_pingouin, monstresRampants[i], _pingouin.HitBox))
                        {
                            _pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                            MediaPlayer.Play(monsterTouchPingouin);
                        }
                    }
                }

                // Collision des volants avec le pingouin
                for (int i = 0; i < monstresVolants.Count; i++)
                {
                    if (!monstresVolants[i].IsDied)
                    {
                        if (Collision.IsCollidingMonstre(_pingouin, monstresVolants[i], _pingouin.HitBox))
                        {
                            _pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                        }
                    }
                }

                for (int i = 0; i < _posiCoins.Length; i++)
                {
                    if (coins[i].etat == 0)
                    {
                        //Collision de la recompense avec le pingouin
                        if (Collision.IsCollidingRecompense(coins[i], _pingouin.HitBox))
                        {
                            if (_pingouin.CurrentLife == _pingouin.MaxLife)
                            {
                                double randomNb = new Random().NextDouble();
                                if (randomNb > 0.5)
                                {
                                    _pingouin.WalkVelocity *= 0.80;
                                }
                                else
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

                for (int i = 0; i < _posiPartiPortail.Length; i++)
                {
                    if (partiPortail[i].etat == 0)
                    {
                        // Collision des moreau de portail avec le pingouin
                        if (Collision.IsCollidingRecompense(partiPortail[i], _pingouin.HitBox))
                        {
                            _partiRecolleter += 1;
                            partiPortail[i].etat = 1;
                        }
                    }
                }
                // Collision avec le portail de changement de niveau
                if (openingPortal.etat == 0)
                {
                    // Collision des moreau de portail avec le pingouin
                    if (Collision.IsCollidingRecompense(openingPortal, _pingouin.HitBox))
                    {
                        _myGame.clicWin = true;
                    }
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

            // Affichage des boules de neiges
            for (int i = 0; i < _snowballs.Length; i++)
            {
                _snowballs[i].Affiche(_myGame);
            }

            // Affichage du chrono
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono : {Chrono.AffichageChrono(Chrono.chrono)}", _positionChrono - new Vector2(20,0), Color.White);

            //Affichage du nombre de parti de portaill recuperer
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_partiRecolleter}" + $"/" + $"{_posiPartiPortail.Length}", _recoltePosition, Color.White);

            //Life
            for (int i = 0; i < _pingouin.CurrentLife; i++)
            {
                _myGame.SpriteBatch.Draw(_heartSprite, _heartsPositions[i], Color.White);
            }

            //Fox
            for (int i = 0; i < monstresRampants.Count; i++)
            {
                monstresRampants[i].Affiche(_myGame);
            }

            //Trap
            for (int i = 0; i < traps.Count; i++)
            {
                traps[i].Affiche(_myGame);
            }

            //Eagle
            for (int i = 0; i < monstresVolants.Count; i++)
            {
                monstresVolants[i].Affiche(_myGame);
            }

            //Affichage des recompenses si elle n'as pas ete prise
            for (int i = 0; i< _posiCoins.Length; i++)
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
                    _myGame.SpriteBatch.Draw(partiPortail[i].Sprite, partiPortail[i].Position, 0, new Vector2((float)0.35));
                }
            }
            if (openingPortal.etat == 0)
            {
                _myGame.SpriteBatch.Draw(openingPortal.Sprite, openingPortal.Position, 0, new Vector2(2));
            }
            if (Chrono.chrono <2)
            {
                _myGame.SpriteBatch.Draw(closingPortal.Sprite, closingPortal.Position, 0, new Vector2(2));
            }

            // Debug collision
            _myGame.SpriteBatch.DrawRectangle(_hitBoxPingouin, Color.Blue);

            for (int i=0; i<_posiCoins.Length; i++)
            {
                if (coins[i].etat == 0)
                {
                    _myGame.SpriteBatch.DrawRectangle(coins[i].RectangleSprite, Color.YellowGreen);
                }
            }

            // Debug Position
            _myGame.SpriteBatch.DrawString(Game1.police, "x : " + $"{Math.Round(_pingouin.Position.X, 0)}", _recoltePosition + new Vector2(0, 100), Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, "y : " + $"{Math.Round(_pingouin.Position.Y, 0)}", _recoltePosition + new Vector2(0, 170), Color.White);


            _myGame.SpriteBatch.End();
        }
    }
}
