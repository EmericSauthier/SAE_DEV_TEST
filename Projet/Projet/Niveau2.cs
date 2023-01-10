using Microsoft.Xna.Framework;
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
    internal class Niveau2 : GameScreen
    {
        private Game1 _myGame;

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
        private TiledMapTileLayer _groundLayer;
        private TiledMapTileLayer _deadLayer;

        //JEU
        private Camera _camera;
        public static float scale;

        // Chrono
        private Vector2 _positionChrono;

        // ENTITE
        private Pingouin _pingouin;
        public int _largeurPingouin = 50, _hauteurPingouin = 40; // à déplacer ?
        private Rectangle _hitBoxPingouin;
        // Fox
        MonstreRampant[] _monstresRampants;
        MonstreRampant _fox1;
        public bool isFox1Died;
        private Vector2[] _posiMonstreRampant;

        // Piège
        Trap _ceilingTrap1;

        //Recompense
        Vector2[] _posiCoins;
        Recompenses[] coins;
        public int largeurRecompense1 = 10, hauteurRecompense1 = 10;

        // Life
        private Texture2D _heartSprite;
        private Vector2[] _heartsPositions;

        //Debug rectangle
        private Rectangle rFox;
        private Rectangle rTrap;
        private Rectangle rKillingFox;
        private Rectangle rRecompense;

        //Portail
        private int _partiRecolleter;
        private Vector2 _recoltePosition;
        Recompenses[] partiPortail;
        Recompenses openingPortal;
        Recompenses closingPortal;
        Vector2[] _posiPartiPortail;

        //Son
        Song recupAllPortalSound;
        Song coinSound;
        Song monsterTouchPingouin;
        Song trapTouchPingouin;

        // Tableau de boule de neige
        private Snowball[] _snowballs;
        private Texture2D _snowballTexture;

        public Niveau2(Game1 game) : base(game)
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

            // Chrono
            Chrono.InitializeChronos();

            // Initialisation du pingouin et de sa position
            _pingouin = new Pingouin(500, 802, scale);

            if (_myGame.reprendre)
            {
                _pingouin = new Pingouin(_myGame.dernierePosiPingouin.X, _myGame.dernierePosiPingouin.Y, scale);
                _myGame.reprendre = false;
            }
            else
            {
                _pingouin = new Pingouin(500, 802, scale);
            }

            // Ennemis
            _posiMonstreRampant = new Vector2[] { new Vector2(1578, 354) };
            _fox1 = new MonstreRampant(new Vector2(1170, 850), "fox", 0.8, 12);
            isFox1Died = false;

            // Traps
            _ceilingTrap1 = new Trap(new Vector2(1302, 1027), "press");

            //Recompenses
            _posiCoins = new Vector2[] { new Vector2(986, 1122), new Vector2(986 + 50, 1122), new Vector2(1086, 1122),
                new Vector2(1086 + 50, 1122), new Vector2(2440, 642), new Vector2(2390, 642), new Vector2(1646, 642),
                new Vector2(1696, 642), new Vector2(212, 92), new Vector2(262, 92), new Vector2(172, 92), new Vector2(312, 92),
                new Vector2(721, 250), new Vector2(771, 250), new Vector2(821, 250) };
            coins = new Recompenses[_posiCoins.Length];
            int x = 986;
            int y = 1122;
            for (int i = 0; i < _posiCoins.Length; i++)
            {
                coins[i] = new Recompenses(_posiCoins[i], "piece", 0);
            }

            // Life
            _heartsPositions = new Vector2[3];

            //Portail
            _posiPartiPortail = new Vector2[] { new Vector2(52, 514), new Vector2(1878, 1054), new Vector2(3170, 1122), new Vector2(780, 99), new Vector2(2430, 292) };
            _partiRecolleter = 0;
            partiPortail = new Recompenses[_posiPartiPortail.Length];
            for (int i = 0; i < _posiPartiPortail.Length; i++)
            {
                partiPortail[i] = new Recompenses(_posiPartiPortail[i], "portal", 0);
            }
            openingPortal = new Recompenses(new Vector2(3054, 322), "portal", 1);
            closingPortal = new Recompenses(new Vector2(400, 770), "portal", 0);

            _snowballs = new Snowball[0];

            base.Initialize();
        }
        public override void LoadContent()
        {
            // Chargement de la map et du TileLayer du sol/décor
            _tiledMap = Content.Load<TiledMap>("Maps/desertMap");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _groundLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Ground");
            _deadLayer = _tiledMap.GetLayer<TiledMapTileLayer>("DeadZone");

            // Chargement du sprite du pingouin
            _pingouin.Perso = new AnimatedSprite(Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader()));

            // Chargement de la texture de la boule de neige
            _manager.SnowballTexture = this.Content.Load<Texture2D>("Perso/snowball");

            // Chargement du sprite du renard
            SpriteSheet foxSprite = Content.Load<SpriteSheet>("Ennemis_pieges/fox.sf", new JsonContentLoader());
            _fox1.LoadContent(foxSprite);

            // Chargement du sprite du piège
            SpriteSheet ceilingTrapSprite = Content.Load<SpriteSheet>("Ennemis_pieges/ceilingTrap.sf", new JsonContentLoader());
            _ceilingTrap1.LoadContent(ceilingTrapSprite);

            // Chargement du sprite de la recompense
            SpriteSheet spriteCoin = Content.Load<SpriteSheet>("Decors/spritCoin.sf", new JsonContentLoader());
            for (int i = 0; i < _posiCoins.Length; i++)
            {
                coins[i].LoadContent(spriteCoin);
            }

            // Chargement de la texture des coeurs
            _heartSprite = Content.Load<Texture2D>("Life/heart");

            //Chargement du sprite du portail
            SpriteSheet spritePortal = Content.Load<SpriteSheet>("Decors/portal.sf", new JsonContentLoader());
            for (int i = 0; i < _posiPartiPortail.Length; i++)
            {
                partiPortail[i].LoadContent(spritePortal);
            }
            openingPortal.LoadContent(spritePortal);
            closingPortal.LoadContent(spritePortal);

            //Audio
            coinSound = Content.Load<Song>("Audio/coinSound");
            recupAllPortalSound = Content.Load<Song>("Audio/recupAllPortal");
            monsterTouchPingouin = Content.Load<Song>("Audio/monsterTouchPingouin");
            trapTouchPingouin = Content.Load<Song>("Audio/trapTouchPingouin");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine(_pingouin.Position);
            
            // GameManager
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //CONDITION POUR AFFICHER LE PORTAIL DE SORTIE
            if (_partiRecolleter == _posiPartiPortail.Length && openingPortal.etat==1)
            {
                openingPortal.etat = 0;
                MediaPlayer.Play(recupAllPortalSound);
            }

            //CONDITION POUR ALLER SUR LE MENU DU JEU
            if (_keyboardState.IsKeyDown(Keys.Tab))
            {
                _myGame.pause = !_myGame.pause;
            }
            else if (!_myGame.pause || _myGame.reprendre)
            {
                //Code cheat
                if (_keyboardState.IsKeyDown(Keys.C))
                {
                    _partiRecolleter = _posiPartiPortail.Length; //L'entiereté des parti de portail est récolleter
                    for (int i=0; i<_posiPartiPortail.Length; i++)
                    {
                        partiPortail[i].etat = 0;
                    }
                }
                if (_keyboardState.IsKeyDown(Keys.Insert))
                {
                    _pingouin.Position = new Vector2(500, 802); //Le pingouin est tp a son point de départ
                }
                if (_keyboardState.IsKeyDown(Keys.V))
                {
                    _pingouin.CurrentLife = 3;//Le pingouin récupere toute sa vie
                }
                if (_keyboardState.IsKeyDown(Keys.End))
                {
                    _pingouin.Position = new Vector2(3054, 322); //Le pingouin est tp a la zone de fin
                }
                if (_keyboardState.IsKeyDown(Keys.Up))
                {
                    _pingouin.Position += new Vector2(0, -5);
                }
                
                
                // Map
                _tiledMapRenderer.Update(gameTime);

                // Camera
                _camera.Update(gameTime, _pingouin);

                // Pingouin
                _myGame.dernierePosiPingouin = new Vector2(_pingouin.Position.GetHashCode()); //envoie dans game 1 la position du pingouin pour pouvoir reprendre a la meme position

                _manager.Update(_keyboardState, _pingouin, ref _snowballs, _groundLayer, deltaSeconds);

                // Chrono
                Chrono.UpdateChronos(deltaSeconds);
                _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

                // Ennemis
                _fox1.RightLeftMove(ref Chrono.chronoDepFox);
                _fox1.Sprite.Update(deltaSeconds);

                // Recompense
                for (int i = 0; i < _posiCoins.Length; i++)
                {
                    coins[i].Sprite.Play("coin");
                    coins[i].Sprite.Update(deltaSeconds);
                }

                //Compteur morceau de portail recolleter
                _recoltePosition = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2+50);

                //Portail
                openingPortal.Sprite.Play("openingPortal");
                closingPortal.Sprite.Play("closingPortal");
                openingPortal.Sprite.Update(deltaSeconds);
                closingPortal.Sprite.Update(deltaSeconds);
                for (int i = 0; i < _posiPartiPortail.Length; i++)
                {
                    partiPortail[i].Sprite.Play("portal");
                    partiPortail[i].Sprite.Update(deltaSeconds);
                }

                // Traps
                _ceilingTrap1.PressActivation(ref Chrono.chronoTrap);
                _ceilingTrap1.Sprite.Update(deltaSeconds);

                // Lifes
                for (int i = 0; i < _pingouin.MaxLife; i++)
                {
                    _heartsPositions[i] = new Vector2(_camera.CameraPosition.X - LARGEUR_FENETRE / 2, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);
                    _heartsPositions[i] += new Vector2(50 * i, 0);
                }

                if (Collision.IsCollidingTrap(_ceilingTrap1, _pingouin.HitBox))
                {
                    _pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                    MediaPlayer.Play(trapTouchPingouin);
                }
                // Collision du monstre avec le pingouin
                if (!isFox1Died)
                {
                    if (Collision.IsCollidingMonstre(_pingouin, _fox1, _hitBoxPingouin))
                    {
                        _pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                        MediaPlayer.Play(monsterTouchPingouin);
                    }
                }


                for (int i = 0; i < _posiCoins.Length; i++)
                {
                    if (coins[i].etat == 0)
                    {
                        //Collision de la recompense avec le pingouin
                        if (Collision.IsCollidingRecompense(coins[i], _hitBoxPingouin))
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
                        //Collision des moreau de portail avec le pingouin
                        if (Collision.IsCollidingRecompense(partiPortail[i], _pingouin.HitBox))
                        {
                            _partiRecolleter += 1;
                            partiPortail[i].etat = 1;
                        }
                    }
                }
                //Collision avec le portail de changement de niveau
                if (openingPortal.etat == 0)
                {
                    //Collision des moreau de portail avec le pingouin
                    if (Collision.IsCollidingRecompense(openingPortal, _pingouin.HitBox))
                    {
                        _myGame.clicWin = true;
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

            // Affichage du chrono
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono : {Chrono.AffichageChrono(Chrono.chrono)}", _positionChrono - new Vector2(20, 0), Color.White);
            //_myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Trap : {Math.Round(_chronoTrap1, 2)}", _positionChrono + new Vector2(-100, 50), Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Invincibility : {Math.Round(Chrono.chronoInvincibility, 2)}", _positionChrono + new Vector2(-170, 100), Color.White);

            //Affichage du nombre de parti de portaill recuperer
            _myGame.SpriteBatch.DrawString(Game1.police, $"{_partiRecolleter}" + $"/" + $"{_posiPartiPortail.Length}", _recoltePosition, Color.White);

            //Life
            for (int i = 0; i < _pingouin.CurrentLife; i++)
            {
                _myGame.SpriteBatch.Draw(_heartSprite, _heartsPositions[i], Color.White);
            }

            // Affichage des ennemis et des pièges
            if (!isFox1Died)
            {
                _myGame.SpriteBatch.Draw(_fox1.Sprite, _fox1.Position, 0, new Vector2(3, 3));
            }
            _myGame.SpriteBatch.Draw(_ceilingTrap1.Sprite, _ceilingTrap1.Position, 0, new Vector2(1, 1));

            //Affichage des recompenses si elle n'as pas ete prise
            for (int i = 0; i < _posiCoins.Length; i++)
            {
                if (coins[i].etat == 0)
                {
                    _myGame.SpriteBatch.Draw(coins[i].Sprite, coins[i].Position, 0, new Vector2((float)0.15));
                }
            }

            //Affichage des parti du portail
            for (int i=0; i < _posiPartiPortail.Length; i++)
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
            if (Chrono.chrono<2)
            {
                _myGame.SpriteBatch.Draw(closingPortal.Sprite, closingPortal.Position, 0, new Vector2(2));
            }

            // Debug collision
            _myGame.SpriteBatch.DrawRectangle(_hitBoxPingouin, Color.Blue);
            _myGame.SpriteBatch.DrawRectangle(rTrap, Color.Orange);
            for (int i = 0; i < _posiCoins.Length; i++)
            {
                if (coins[i].etat == 0)
                {
                    _myGame.SpriteBatch.DrawRectangle(rRecompense, Color.YellowGreen);
                }
            }

            if (!isFox1Died)
            {
                _myGame.SpriteBatch.DrawRectangle(rFox, Color.Red);
                _myGame.SpriteBatch.DrawRectangle(rKillingFox, Color.DarkOrange);
            }

            _myGame.SpriteBatch.End();
        }
    }
}
