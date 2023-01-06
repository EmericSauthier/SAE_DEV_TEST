using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        // Fox
        MonstreRampant[] _monstresRampants;
        MonstreRampant _fox1;
        public int _largeurFox1 = 19*3, _hauteurFox1 = 14*3; // à déplacer 

        // Traps
        Trap _ceilingTrap1;
        private float _chronoTrap1, _chronoInvincibility;
        public static bool _canCollidingTrap;
        public int _largeurTrap1 = (64/2), _hauteurTrap1 = 64-20; // à déplacer 

        //Recompense
        Recompenses recompense;
        public int largeurRecompense1 = 10, hauteurRecompense1 = 10;
        bool recompensePrise = false;

        // GameManager
        private bool _gameOver;
        private KeyboardState _keyboardState;
        private TiledMapTileLayer _mapLayer;

        // Chrono
        private Vector2 _positionChrono;
        private float _chrono;
        private float _chronoDep;

        // Life
        private Life _pingouinLife;
        private Texture2D _heartSprite;
        private Vector2[] _heartsPositions;

        //Debug rectangle
        private Rectangle rPingouin;
        private Rectangle rFox;
        private Rectangle rTrap;


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

            // Life
            _pingouinLife = new Life(3);

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
            _fox1 = new MonstreRampant(new Vector2(1150, 850), "fox", 1, 2.5);

            // Traps
            _ceilingTrap1 = new Trap(new Vector2(1480, 800));

            //Recompenses
            recompense = new Recompenses(new Vector2(1150, 850), "piece");

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

            // Life
            _heartSprite = Content.Load<Texture2D>("Life/heart");

            // Chargement du sprite de la recompense
            SpriteSheet spriteCoin = Content.Load<SpriteSheet>("Decors/spritCoin.sf", new JsonContentLoader());
            recompense.LoadContent(spriteCoin);

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
                _pingouin.Perso.Update(deltaSeconds);

                // Chrono
                _chrono += deltaSeconds;
                _positionChrono = new Vector2(_camera.CameraPosition.X + LARGEUR_FENETRE / 2 - 190, _camera.CameraPosition.Y - HAUTEUR_FENETRE / 2);

                // Ennemis
                _chronoDep += deltaSeconds;
                _fox1.RightLeftMove(ref _chronoDep);
                _fox1.Sprite.Update(deltaSeconds);

                // Traps
                _chronoTrap1 += deltaSeconds;
                _chronoInvincibility += deltaSeconds;
                _ceilingTrap1.PressActivation(ref _chronoTrap1, ref _canCollidingTrap);
                _ceilingTrap1.Sprite.Update(deltaSeconds);

                // Lifes
                for (int i = 0; i < _pingouinLife.MaxLife; i++)
                {
                    _heartsPositions[i] = _positionChrono - new Vector2(500, 0);
                    _heartsPositions[i] += new Vector2(50*i, 0);
                }


                // Collisions
                if (Collision.IsCollidingTrap(_pingouin, _largeurPingouin, _hauteurPingouin, _ceilingTrap1, _largeurTrap1, _hauteurTrap1, _canCollidingTrap, ref rTrap, ref rPingouin))
                {
                    _pingouinLife.TakeDamage(1, ref _chronoInvincibility);
                }
                // Collision du monstre avec le pingouin
                if (Collision.IsCollidingMonstreRampant(_pingouin, _largeurPingouin, _hauteurPingouin, _fox1, _largeurFox1, _hauteurFox1, ref rFox, ref rPingouin))
                {
                    _pingouinLife.TakeDamage(1, ref _chronoInvincibility);
                }
                if (!recompensePrise)
                {
                    //Collision de la recompense avec le pingouin
                    if (Collision.IsCollidingRecompense(_pingouin, _largeurPingouin, _hauteurPingouin, recompense, largeurRecompense1, hauteurRecompense1, scale))
                    {
                        _pingouin.WalkVelocity *= 0.80;
                        recompensePrise = true;
                    }
                }
                

                // Mort
                if (_pingouinLife.CurrentLife <= 0)
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
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono : {(int)_chrono}", _positionChrono, Color.White);
            //_myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Trap : {Math.Round(_chronoTrap1, 2)}", _positionChrono + new Vector2(-100, 50), Color.White);
            _myGame.SpriteBatch.DrawString(Game1.police, $"Chrono Invincibility : {Math.Round(_chronoInvincibility, 2)}", _positionChrono + new Vector2(-150, 100), Color.White);

            //Life
            for (int i = 0; i < _pingouinLife.CurrentLife; i++)
            {
                _myGame.SpriteBatch.Draw(_heartSprite, _heartsPositions[i], Color.White);
            }

            // Affichage des ennemis et des pièges
            _myGame.SpriteBatch.Draw(_fox1.Sprite, _fox1.Position, 0, new Vector2(3, 3));
            _myGame.SpriteBatch.Draw(_ceilingTrap1.Sprite, _ceilingTrap1.Position, 0, new Vector2(1, 1));

            if (!recompensePrise)
            {
                //Affichage des recompenses si elle n'as pas ete prise
                _myGame.SpriteBatch.Draw(recompense.Sprite, recompense.Position, 0, new Vector2((float)0.15));
            }
            

            // Debug collision
            _myGame.SpriteBatch.DrawRectangle(rFox, Color.Red);
            _myGame.SpriteBatch.DrawRectangle(rPingouin, Color.Blue);
            _myGame.SpriteBatch.DrawRectangle(rTrap, Color.Orange);

            _myGame.SpriteBatch.End();
        }
    }
}
