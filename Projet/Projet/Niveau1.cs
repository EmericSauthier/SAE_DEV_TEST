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
    internal class Niveau1 : GameScreen
    {
        private Game1 _myGame;

        public const int LARGEUR_FENETRE = 1000, HAUTEUR_FENETRE = 800;
        private GraphicsDeviceManager _graphics;

        //SOURIS POUR GERER CLIC
        private MouseState _mouseState;

        //MAP
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        //JEU
        private Camera _camera;
        public static float scale;

        // ENTITE
        private Pingouin _pingouin;
        public int _largeurPingouin = 50, _hauteurPingouin = 40; // à déplacer ?
        private Rectangle _hitBoxPingouin;
        // Fox
        MonstreRampant[] _monstresRampants;
        MonstreRampant _fox1;
        public int _largeurFox1 = 19*3, _hauteurFox1 = 14*3; // à déplacer 
        public bool isFox1Died;

        // Traps
        Trap _ceilingTrap1;
        private float _chronoTrap1, _chronoInvincibility;
        public static bool _canCollidingTrap;
        public int _largeurTrap1 = (64/2), _hauteurTrap1 = 64-20; // à déplacer 

        //Recompense
        Recompenses []coins;
        public int largeurRecompense1 = 10, hauteurRecompense1 = 10;
        Song coinSound;

        // GameManager
        private bool _gameOver;
        private KeyboardState _keyboardState;
        private TiledMapTileLayer _mapLayer;

        // Chrono
        private Vector2 _positionChrono;
        private float _chrono;
        private float _chronoDep;

        // Life
        private Texture2D _heartSprite;
        private Vector2[] _heartsPositions;


        //Debug rectangle
        private Rectangle rFox;
        private Rectangle rTrap;
        private Rectangle rKillingFox;
        private Rectangle rRecompense;

        //BOULE DE NEIGE
        private Texture2D _snowballTexture;
        private Snowball[] _snowballs;


        public Niveau1(Game1 game) : base(game)
        {
            _myGame = game;
        }

        public override void Initialize()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _myGame.Window.Title = "Jeu du pingouin";

            // GameManager
            _gameOver = false;

            // Initialisation du pingouin et de sa position
            _pingouin = new Pingouin(LARGEUR_FENETRE / 2, 500 + (HAUTEUR_FENETRE / 2));

            if (_myGame.reprendre)
            {
                _pingouin = new Pingouin(_myGame._dernierePosiPingouin.X, _myGame._dernierePosiPingouin.Y);
                _myGame.reprendre = false;
            }
            else
            {
                _pingouin = new Pingouin(LARGEUR_FENETRE / 2, 500 + (HAUTEUR_FENETRE / 2));
            }

            // Ennemis
            _fox1 = new MonstreRampant(new Vector2(1170, 850), "fox", 0.5, 6);
            isFox1Died = false;

            // Traps
            _ceilingTrap1 = new Trap(new Vector2(1480, 800));

            //Recompenses
            coins = new Recompenses[4];
            int x = 1150;
            int y = 780;
            for (int i=0; i <4; i++)
            {
                coins[i] = new Recompenses(new Vector2(x+50*i, y), "piece", 0);
            }

            // Camera
            scale = (float)0.5;
            _camera = new Camera();
            _camera.Initialize(_myGame.Window, GraphicsDevice, LARGEUR_FENETRE, HAUTEUR_FENETRE);

            // Chrono
            _chrono = 0;
            _chronoDep = 0;
            _chronoInvincibility = 0;

            // Life
            _heartsPositions = new Vector2[3];

            base.Initialize();
        }
        public override void LoadContent()
        {
            // Chargement de la map et du TileLayer du sol/décor
            _tiledMap = Content.Load<TiledMap>("Maps/snowmap1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _mapLayer = _tiledMap.GetLayer<TiledMapTileLayer>("Ground");

            // Chargement du sprite du pingouin
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            _pingouin.Perso = new AnimatedSprite(spriteSheet);

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

            // Life
            _heartSprite = Content.Load<Texture2D>("Life/heart");

            // Chargement de la texture de la boule de neige
            _snowballTexture = this.Content.Load<Texture2D>("Perso/snowball");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            // GameManager
            _keyboardState = Keyboard.GetState();
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                _myGame._dernierePosiPingouin = new Vector2(_pingouin.Position.GetHashCode()); //envoie dans game 1 la position du pingouin pour pouvoir reprendre a la meme position
                _pingouin.Move(_gameOver, _keyboardState, _mapLayer);

                if (_keyboardState.IsKeyDown(Keys.Enter))
                {
                    _pingouin.Perso.Play("attack");
                }

                _pingouin.Perso.Update(deltaSeconds);

                // Chrono
                _chrono += deltaSeconds;
                _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

                // Ennemis
                _chronoDep += deltaSeconds;
                _fox1.RightLeftMove(ref _chronoDep);
                _fox1.Sprite.Update(deltaSeconds);

                // Recompense
                for (int i =0; i<4; i++)
                {
                    _chronoDep += deltaSeconds;
                    coins[i].Sprite.Play("coin");
                    coins[i].Sprite.Update(deltaSeconds);
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
                    _heartsPositions[i] += new Vector2(50*i, 0);
                }

                // Collisions
                _hitBoxPingouin = new Rectangle((int)_pingouin.Position.X - 25, (int)_pingouin.Position.Y - 15, (int)(_largeurPingouin), (int)(_hauteurPingouin));

                if (Collision.IsCollidingTrap(_ceilingTrap1, _largeurTrap1, _hauteurTrap1, _canCollidingTrap, ref rTrap, _hitBoxPingouin))
                {
                    _pingouin.TakeDamage(1, ref _chronoInvincibility);
                }
                // Collision du monstre avec le pingouin
                if (!isFox1Died)
                {
                    if (Collision.IsCollidingMonstreRampant(_pingouin, _fox1, _largeurFox1, _hauteurFox1, ref isFox1Died, ref rFox, ref rKillingFox, _hitBoxPingouin))
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
                            if (_pingouinLife.CurrentLife == _pingouinLife.MaxLife)
                            {
                                _pingouin.WalkVelocity *= 0.80;
                                coins[i].etat = 1;
                                MediaPlayer.Play(coinSound);
                            }
                            else
                            {
                                coins[i].etat = 1;
                                _pingouinLife.Heal(1);
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

            // Application du zoom de la camera
            _tiledMapRenderer.Draw(_camera.OrthographicCamera.GetViewMatrix());
            _myGame.SpriteBatch.Begin(transformMatrix: _camera.OrthographicCamera.GetViewMatrix());

            // Affichage du pingouin
            _myGame.SpriteBatch.Draw(_pingouin.Perso, _pingouin.Position, 0, new Vector2(scale));

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 40 * scale, _pingouin.Position.Y + 60 * scale, Color.Green, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 40 * scale, _pingouin.Position.Y + 60 * scale, Color.Green, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 40 * scale, _pingouin.Position.Y - 40 * scale, Color.Orange, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 40 * scale, _pingouin.Position.Y - 40 * scale, Color.Orange, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y + 50 * scale, Color.Red, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y, Color.Red, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X + 50 * scale, _pingouin.Position.Y - 30 * scale, Color.Red, 5);

            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y + 50 * scale, Color.Blue, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y, Color.Blue, 5);
            _myGame.SpriteBatch.DrawPoint(_pingouin.Position.X - 50 * scale, _pingouin.Position.Y - 30 * scale, Color.Blue, 5);

            // Affichage du chrono
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono : {Chrono.AffichageChrono(_chrono)}", _positionChrono - new Vector2(20,0), Color.White);
            //_myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Trap : {Math.Round(_chronoTrap1, 2)}", _positionChrono + new Vector2(-100, 50), Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Invincibility : {Math.Round(_chronoInvincibility, 2)}", _positionChrono + new Vector2(-170, 100), Color.White);

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
            for (int i = 0; i<4; i++)
            {
                if (coins[i].etat == 0)
                {
                    _myGame.SpriteBatch.Draw(coins[i].Sprite, coins[i].Position, 0, new Vector2((float)0.15));
                }
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
            
            if (!isFox1Died)
            {
                _myGame.SpriteBatch.DrawRectangle(rFox, Color.Red);
                _myGame.SpriteBatch.DrawRectangle(rKillingFox, Color.DarkOrange);
            }

            _myGame.SpriteBatch.End();
        }
    }
}
