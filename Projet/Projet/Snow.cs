using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;

namespace Projet
{
    internal class Snow : GameScreen
    {
        private Game1 _myGame;

        //Activation cheat
        private bool _cheat;
        
        // Fenêtre
        public const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;

        // GameManager
        private GameManager _manager;

        // Gestion des entrées
        private KeyboardState _keyboardState;

        // Variables de map
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private TiledMapTileLayer _spikesLayer;
        private Texture2D _fondSnow;

        // JEU
        private Camera _camera;
        public static float scale;

        // Chrono
        private Vector2 _positionChrono;

        // ENTITE
        private List<MonstreRampant> _monstresRampants;
        private List<MonstreVolant> _monstresVolants;
        private List<Trap> _traps;
        private Pingouin _pingouin;

        // Recompense
        private Recompenses[] _coins;
        private Vector2[] _posiCoins;
        public int largeurRecompense1 = 10, hauteurRecompense1 = 10;

        // Life
        private Texture2D _heartSprite;
        private Vector2[] _heartsPositions;

        //Portail
        private Vector2 _recolteesPosition;
        private int _partieRecoletee;
        private Recompenses[] _partiesPortail;
        private Recompenses _openingPortal;
        private Recompenses _closingPortal;
        Vector2[] _posiPartiPortail;

        // Audio
        private SoundEffect recupAllPortalSound;
        private SoundEffect coinSound;
        private Song soundtrack;


        // Tableau de boule de neige
        private Snowball[] _snowballs;

        public Snow(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _myGame.Window.Title = "Man-chaud";

            // Etat de la partie
            _manager = new GameManager();
            _myGame.nivActu = 2;

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
            _monstresRampants = new List<MonstreRampant>();
            _monstresRampants.Add(new MonstreRampant(new Vector2(1170, 850), "fox", 0.5, 5));
            _monstresRampants.Add(new MonstreRampant(new Vector2(2430, 725), "fox", 0.4, 5));
            _monstresRampants.Add(new MonstreRampant(new Vector2(3660, 500), "fox", 0.4, 5));
            _monstresRampants.Add(new MonstreRampant(new Vector2(3690, 245), "fox", 0.4, 5));
            _monstresRampants.Add(new MonstreRampant(new Vector2(4580, 785), "fox", 0.5, 5));
            _monstresRampants.Add(new MonstreRampant(new Vector2(5480, 595), "fox", 0.4, 5));
            // Tableau monstre volant
            _monstresVolants = new List<MonstreVolant>();
            _monstresVolants.Add(new MonstreVolant(new Vector2(1000, 500), "eagle", 1, 12));
            _monstresVolants.Add(new MonstreVolant(new Vector2(2500, 100), "eagle", 1, 12));
            _monstresVolants.Add(new MonstreVolant(new Vector2(5000, 500), "eagle", 1, 12));
            // Tableau Traps
            _traps = new List<Trap>();
            _traps.Add(new Trap(new Vector2(2728, 897), "press"));
            _traps.Add(new Trap(new Vector2(1618, 767), "press"));
            _traps.Add(new Trap(new Vector2(3200, 897), "press"));
            _traps.Add(new Trap(new Vector2(4300, 735), "press"));

            // Recompenses
            _posiCoins = new Vector2[] { new Vector2(850, 900), new Vector2(1000, 850), new Vector2(1870, 650), new Vector2(3310, 400)
                , new Vector2(3050, 200), new Vector2(4020, 190), new Vector2(3750, 850), new Vector2(3010, 930), new Vector2(5900, 650)};
            _coins = new Recompenses[_posiCoins.Length];
            for (int i=0; i <_posiCoins.Length; i++)
            {
                _coins[i] = new Recompenses(_posiCoins[i], "piece", 0);
            }

            // Life
            _heartsPositions = new Vector2[3];

            // Portail
            _posiPartiPortail = new Vector2[] { new Vector2(1070, 642), new Vector2(2801, 300) };
            _partieRecoletee = 0;
            _partiesPortail = new Recompenses[_posiPartiPortail.Length];
            for (int i = 0; i < _posiPartiPortail.Length; i++)
            {
                _partiesPortail[i] = new Recompenses(_posiPartiPortail[i], "portal", 0);
            }
            _openingPortal = new Recompenses(new Vector2(6246, 740), "portal", 1);
            _closingPortal = new Recompenses(new Vector2(364,900), "portal", 0);

            _snowballs = new Snowball[0];

            base.Initialize();
        }
        public override void LoadContent()
        {
            // Chargement de la map et du TileLayer du sol/décor
            _tiledMap = Content.Load<TiledMap>("Maps/snowmap1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _spikesLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Spikes");

            _fondSnow = Content.Load<Texture2D>("Decors/fondGame1");

            // Chargement du sprite du pingouin
            _pingouin.Perso = new AnimatedSprite(Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader()));

            // Chargement de la texture de la boule de neige
            _manager.SnowballTexture = Content.Load<Texture2D>("Perso/snowball");

            // Chargement du sprite du renard
            SoundEffect foxDeath = Content.Load<SoundEffect>("Audio/foxDeath");
            SpriteSheet foxSprite = Content.Load<SpriteSheet>("Ennemis_pieges/fox.sf", new JsonContentLoader());
            for (int i = 0; i < _monstresRampants.Count; i++)
            {
                _monstresRampants[i].LoadContent(foxSprite, foxDeath);
            };

            // Chargement texture eagle
            SpriteSheet eagleSprite = Content.Load<SpriteSheet>("Ennemis_pieges/eagle.sf", new JsonContentLoader());
            for (int i = 0; i < _monstresVolants.Count; i++)
            {
                _monstresVolants[i].LoadContent(eagleSprite);
            }

            // Chargement du sprite du piège
            SpriteSheet ceilingTrapSprite = Content.Load<SpriteSheet>("Ennemis_pieges/ceilingTrap.sf", new JsonContentLoader());
            for (int i = 0; i < _traps.Count; i++)
            {
                _traps[i].LoadContent(ceilingTrapSprite);
            }

            // Chargement du sprite de la recompense
            SpriteSheet spriteCoin = Content.Load<SpriteSheet>("Decors/spritCoin.sf", new JsonContentLoader());
            for (int i =0; i< _posiCoins.Length; i++)
            {
                _coins[i].LoadContent(spriteCoin);
            }

            // Chargement de la texture des coeurs
            _heartSprite = Content.Load<Texture2D>("Life/heart");

            // Chargement du sprite du portail
            SpriteSheet spritePortal = Content.Load<SpriteSheet>("Decors/portal.sf", new JsonContentLoader());
            for (int i=0; i < _posiPartiPortail.Length; i++)
            {
                _partiesPortail[i].LoadContent(spritePortal);
            }
            _openingPortal.LoadContent(spritePortal);
            _closingPortal.LoadContent(spritePortal);


            // Chargement des audio
            coinSound = Content.Load<SoundEffect>("Audio/coinSound");
            recupAllPortalSound = Content.Load<SoundEffect>("Audio/recupAllPortal");
            soundtrack = Content.Load<Song>("Audio/soundtrack2");

            // Chargement des audio
            _manager.CoinSong = Content.Load<SoundEffect>("Audio/coinSound");
            _manager.PortalSong = Content.Load<SoundEffect>("Audio/recupAllPortal");
            _manager.MonstreSong = Content.Load<SoundEffect>("Audio/monsterTouchPingouin");
            _manager.TrapSong = Content.Load<SoundEffect>("Audio/trapTouchPingouin");
            _manager.ThrowSnowball = Content.Load<SoundEffect>("Audio/snowballLancer");
            _manager.HitSnowball = Content.Load<SoundEffect>("Audio/snowballTouch");

            MediaPlayer.Play(soundtrack);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            
            // GameManager
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // CONDITION POUR GAGNER
            if (_partieRecoletee ==_posiPartiPortail.Length)
            {
                _openingPortal.etat = 0;
                recupAllPortalSound.Play();
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
                    _cheat = true;
                if (_cheat)
                {
                    if (_keyboardState.IsKeyDown(Keys.C))
                    {
                        _partieRecoletee = _posiPartiPortail.Length; //L'entiereté des parti de portail est récolleter
                        for (int i = 0; i < _posiPartiPortail.Length; i++)
                        {
                            _partiesPortail[i].etat = 0;
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

                _manager.Update(_myGame, _keyboardState, _pingouin, ref _snowballs, ref _monstresRampants, ref _monstresVolants, _tiledMap, deltaSeconds, _spikesLayer, _traps);

                // Chrono
                Chrono.UpdateChronos(deltaSeconds);
                _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

                // Ennemis
                // Rampants
                for (int i = 0; i < _monstresRampants.Count; i++)
                {
                    _monstresRampants[i].RightLeftMove(gameTime);
                    _monstresRampants[i].Sprite.Update(deltaSeconds);
                    _monstresRampants[i].UpdateBoxes();
                }

                // Volants
                for (int i = 0; i < _monstresVolants.Count; i++)
                {
                    _monstresVolants[i].Move(gameTime, _pingouin);
                    _monstresVolants[i].Sprite.Update(deltaSeconds);
                    _monstresVolants[i].UpdateBoxes();
                }

                // Recompense
                for (int i =0; i< _posiCoins.Length; i++)
                {
                    _coins[i].Sprite.Play("coin");
                    _coins[i].Sprite.Update(deltaSeconds);
                }

                // Compteur morceau de portail recolleter
                _recolteesPosition = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2 + 50);

                // Portail
                _openingPortal.Sprite.Play("openingPortal");
                _closingPortal.Sprite.Play("closingPortal");
                _openingPortal.Sprite.Update(deltaSeconds);
                _closingPortal.Sprite.Update(deltaSeconds);
                for(int i=0; i < _posiPartiPortail.Length; i++)
                {
                    _partiesPortail[i].Sprite.Play("portal");
                    _partiesPortail[i].Sprite.Update(deltaSeconds);
                }

                // Traps
                for (int i = 0; i < _traps.Count; i++)
                {
                    _traps[i].PressActivation(gameTime);
                    _traps[i].Sprite.Update(deltaSeconds);
                    _traps[i].UpdateBoxes();
                }

                // Lifes
                for (int i = 0; i < _pingouin.MaxLife; i++)
                {
                    _heartsPositions[i] = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2 , _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);
                    _heartsPositions[i] += new Vector2(50 * i, 0);
                }

                for (int i = 0; i < _posiCoins.Length; i++)
                {
                    if (_coins[i].etat == 0)
                    {
                        //Collision de la recompense avec le pingouin
                        if (Collision.IsCollidingRecompense(_coins[i], _pingouin.HitBox))
                        {
                            if (_pingouin.CurrentLife == _pingouin.MaxLife)
                            {
                                double randomNb = new Random().NextDouble();
                                if (randomNb > 0.6)
                                {
                                    _pingouin.WalkVelocity *= 0.80;
                                }
                                else
                                {
                                    _pingouin.WalkVelocity *= 1.20;
                                }
                                _coins[i].etat = 1;
                                coinSound.Play();
                            }
                            else
                            {
                                _coins[i].etat = 1;
                                _pingouin.Heal(1);
                                coinSound.Play();
                            }
                        }
                    }
                }

                for (int i = 0; i < _posiPartiPortail.Length; i++)
                {
                    if (_partiesPortail[i].etat == 0)
                    {
                        // Collision des moreau de portail avec le pingouin
                        if (Collision.IsCollidingRecompense(_partiesPortail[i], _pingouin.HitBox))
                        {
                            _partieRecoletee += 1;
                            _partiesPortail[i].etat = 1;
                        }
                    }
                }
                // Collision avec le portail de changement de niveau
                if (_openingPortal.etat == 0)
                {
                    // Collision des moreau de portail avec le pingouin
                    if (Collision.IsCollidingRecompense(_openingPortal, _pingouin.HitBox))
                    {
                        _myGame.clicWin = true;
                    }
                }
    }
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Fond de map
            _myGame.SpriteBatch.Begin();
            _myGame.SpriteBatch.Draw(_fondSnow, new Vector2(0,0), Color.White);
            _myGame.SpriteBatch.End();

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

            // Affichage du nombre de parti de portaill recuperer
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_partieRecoletee}" + $"/" + $"{_posiPartiPortail.Length}", _recolteesPosition, Color.White);

            // Life
            for (int i = 0; i < _pingouin.CurrentLife; i++)
            {
                _myGame.SpriteBatch.Draw(_heartSprite, _heartsPositions[i], Color.White);
            }

            // Fox
            for (int i = 0; i < _monstresRampants.Count; i++)
            {
                _monstresRampants[i].Affiche(_myGame);
            }

            // Trap
            for (int i = 0; i < _traps.Count; i++)
            {
                _traps[i].Affiche(_myGame);
            }

            // Eagle
            for (int i = 0; i < _monstresVolants.Count; i++)
            {
                _monstresVolants[i].Affiche(_myGame);
            }

            //Affichage des recompenses si elle n'as pas ete prise
            for (int i = 0; i< _posiCoins.Length; i++)
            {
                if (_coins[i].etat == 0)
                {
                    _myGame.SpriteBatch.Draw(_coins[i].Sprite, _coins[i].Position, 0, new Vector2((float)0.15));
                }
            }

            // Affichage des parti du portail
            for(int i =0; i < _posiPartiPortail.Length; i++)
            {
                if (_partiesPortail[i].etat == 0)
                {
                    _myGame.SpriteBatch.Draw(_partiesPortail[i].Sprite, _partiesPortail[i].Position, 0, new Vector2((float)0.35));
                }
            }
            if (_openingPortal.etat == 0)
            {
                _myGame.SpriteBatch.Draw(_openingPortal.Sprite, _openingPortal.Position, 0, new Vector2(2));
            }
            if (Chrono.chrono <1)
            {
                _myGame.SpriteBatch.Draw(_closingPortal.Sprite, _closingPortal.Position, 0, new Vector2(2));
            }

            _myGame.SpriteBatch.End();
        }
    }
}
