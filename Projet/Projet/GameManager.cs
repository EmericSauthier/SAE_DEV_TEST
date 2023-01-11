using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;

namespace Projet
{
    internal class GameManager
    {
        // Touche de déplacement
        public Keys _gauche;
        public Keys _droite;
        public Keys _sauter;
        public Keys _glisser;
        public Keys _attaquer;

        private float _timer;
        private float _timerSpike;
        private Texture2D _snowballTexture;

        private SoundEffect _trapSong;
        private SoundEffect _monstreSong;
        private SoundEffect _coinSong;
        private SoundEffect _portalSong;

        public Texture2D SnowballTexture
        {
            get
            {
                return this._snowballTexture;
            }

            set
            {
                this._snowballTexture = value;
            }
        }
        public SoundEffect TrapSong
        {
            get
            {
                return this._trapSong;
            }

            set
            {
                this._trapSong = value;
            }
        }
        public SoundEffect MonstreSong
        {
            get
            {
                return this._monstreSong;
            }

            set
            {
                this._monstreSong = value;
            }
        }
        public SoundEffect CoinSong
        {
            get
            {
                return this._coinSong;
            }

            set
            {
                this._coinSong = value;
            }
        }
        public SoundEffect PortalSong
        {
            get
            {
                return this._portalSong;
            }

            set
            {
                this._portalSong = value;
            }
        }

        public GameManager()
        {
            _timer = 0;
            _timerSpike = 0;
        }

        public void Update(Game1 game, KeyboardState keyboardState, Pingouin pingouin, ref Snowball[] snowballs, ref List<MonstreRampant> rampants, ref List<MonstreVolant> volants, TiledMap map, float deltaTime, TiledMapTileLayer spikes = null, List<Trap> traps=null)
        {
            TiledMapTileLayer ground = map.GetLayer<TiledMapTileLayer>("Ground");
            TiledMapTileLayer dead = map.GetLayer<TiledMapTileLayer>("DeadZone");

            if (pingouin.CurrentLife <= 0 || Collision.MapCollision(pingouin.CheckBottom(), dead))
            {
                game.goDead = true;
            }
            else
            {
                // Mise à jour des timer
                _timer += deltaTime;
                _timerSpike += deltaTime;

                // Vérifie les entrées et agit en fonction
                InputsManager(keyboardState, pingouin, ref snowballs, ground);

                // Vérifie les collisions des monstres avec les boules de neige et le pingouin
                CollisionWithMonstre(pingouin, ref rampants);
                CollisionWithMonstre(pingouin, ref volants);
                // Vérifie les collisions des boules de neige (avec le décor)
                SnowballsUpdate(ref snowballs, ref rampants, ref volants, ground);
                // Met à jour le pingouin
                pingouin.Update(deltaTime, ground);

                // Vérifie les collisions entre le pingouin et les pièges
                if (traps != null)
                    CollisionWithTrap(pingouin, traps);
                // Vérifie les collisions entre le pingouin et les spikes de la map
                if (spikes != null)
                    CollisionWithTrap(pingouin, spikes);
            }
        }
        public void InputsManager(KeyboardState keyboardState, Pingouin pingouin, ref Snowball[] snowballs, TiledMapTileLayer ground)
        {
            if (keyboardState.IsKeyUp(_glisser))
            {
                pingouin.SlideState = false;
            }

            // Vérification de l'état des flèches droite et gauche
            if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right))
            {
                pingouin.IsMovingLeft = true;
                pingouin.IsMovingRight = false;
                pingouin.Direction = "Left";
            }
            else if (!keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Right))
            {
                pingouin.IsMovingRight = true;
                pingouin.IsMovingLeft = false;
                pingouin.Direction = "Right";
            }
            else
            {
                pingouin.IsMovingLeft = false;
                pingouin.IsMovingRight = false;
                pingouin.Direction = "Right";
            }

            // Vérification de l'état de la touche entrée
            if (keyboardState.IsKeyDown(Game1.attaquer) && snowballs.Length < 5 && _timer >= 1)
            {
                _timer = 0;
                int direction = 1;
                // Joue l'animation d'attaque en fonction du sens du personnage
                if (pingouin.IsMovingLeft)
                {
                    direction = -1;
                }
                pingouin.Animate("attack");

                // Ajoute une boule de neige au tableau
                Snowball[] newSnowballsArray = new Snowball[snowballs.Length + 1];
                for (int i = 0; i < snowballs.Length; i++)
                {
                    newSnowballsArray[i] = snowballs[i];
                }
                Snowball newSnowball = new Snowball(pingouin.Position.X + 50 * pingouin.Scale, pingouin.Position.Y + 10 * pingouin.Scale, pingouin.Scale, _snowballTexture);
                newSnowball.Velocity *= direction;
                newSnowballsArray[newSnowballsArray.Length - 1] = newSnowball;
                snowballs = newSnowballsArray;
            }

            // Si le pingouin saute (touche espace) ou est dans les airs
            if (keyboardState.IsKeyDown(Game1.sauter) || pingouin.Fly)
            {
                if (!Collision.MapCollision(pingouin.CheckTop(), ground))
                {
                    pingouin.Jump(ground);
                }
                else
                {
                    pingouin.JumpState = false;
                }
            }
            // Si le pingouin glisse (flèche du bas)
            else if (keyboardState.IsKeyDown(Game1.glisser))
            {
                int direction = 0;
                if (pingouin.IsMovingRight && !Collision.MapCollision(pingouin.CheckRight(), ground))
                {
                    direction = 1;
                }
                else if (pingouin.IsMovingLeft && !Collision.MapCollision(pingouin.CheckRight(), ground))
                {
                    direction = -1;
                }
                pingouin.Slide(direction);
            }
            // Si le pingouin va à droite (flèche droite uniquement)
            else if ((pingouin.IsMovingRight && !Collision.MapCollision(pingouin.CheckRight(), ground)) || (pingouin.IsMovingLeft && !Collision.MapCollision(pingouin.CheckLeft(), ground)))
            {
                pingouin.Walk();
            }
            // Si aucun mouvement n'est demandé, il reste immobile et joue son animation idle
            else
            {
                pingouin.Animate("idle");
            }
        }
        public void CollisionWithMonstre(Pingouin pingouin, ref List<MonstreRampant> rampants)
        {
            List<MonstreRampant> newRampants = new List<MonstreRampant>();
            for (int i = 0; i < rampants.Count; i++)
            {
                if (!Collision.SpriteCollision(pingouin.HitBox, rampants[i].RectangleKill))
                {
                    newRampants.Add(rampants[i]);
                }
                
                if (Collision.SpriteCollision(pingouin.HitBox, rampants[i].RectangleSprite))
                {
                    if ((rampants[i].IsMovingRight && pingouin.IsMovingRight) || (!rampants[i].IsMovingRight && pingouin.IsMovingRight) || (!rampants[i].IsMovingRight && !pingouin.IsMovingRight && !pingouin.IsMovingLeft))
                    {
                        pingouin.Position -= new Vector2(10, 0);
                    }
                    else
                    {
                        pingouin.Position += new Vector2(10, 0);
                    }
                    pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                    _monstreSong.Play();
                }
            }
            rampants = newRampants;
        }
        public void CollisionWithMonstre(Pingouin pingouin, ref List<MonstreVolant> volants)
        {
            List<MonstreVolant> newVolants = new List<MonstreVolant>();
            for (int i = 0; i < volants.Count; i++)
            {
                if (!Collision.SpriteCollision(pingouin.HitBox, volants[i].RectangleKill))
                {
                    if (Collision.SpriteCollision(pingouin.HitBox, volants[i].RectangleSprite))
                    {
                        if ((volants[i].IsMovingRight && pingouin.IsMovingRight) || (!volants[i].IsMovingRight && pingouin.IsMovingRight))
                        {
                            pingouin.Position -= new Vector2(10, 0);
                        }
                        else
                        {
                            pingouin.Position += new Vector2(10, 0);
                        }

                        if (pingouin.Position.Y > volants[i].Position.Y)
                        {
                            pingouin.Position += new Vector2(0, 10);
                        }
                        pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                        _monstreSong.Play();
                    }
                    else if (Collision.SpriteCollision(pingouin.HitBox, volants[i].RectangleDetection))
                    {
                        volants[i].HasSawPlayer = true;
                    }
                    else
                    {
                        volants[i].HasSawPlayer = false;
                    }
                    newVolants.Add(volants[i]);
                }
            }
            volants = newVolants;
        }
        public void CollisionWithTrap(Pingouin pingouin, List<Trap> traps)
        {
            for (int i = 0; i < traps.Count; i++)
            {
                if (traps[i].CanCollidingTrap && Collision.SpriteCollision(pingouin.HitBox, traps[i].RectangleSprite))
                {
                    pingouin.TakeDamage(1, ref Chrono.chronoInvincibility);
                    _trapSong.Play();
                }
            }
        }
        public void CollisionWithTrap(Pingouin pingouin, TiledMapTileLayer spikes)
        {
            if (spikes != null && Collision.MapCollision(new Point((int)pingouin.Position.X, (int)(pingouin.Position.Y + 50 * pingouin.Scale)), spikes) && _timerSpike >= 2)
            {
                pingouin.CurrentLife -= 1;
                _timerSpike = 0;
                _trapSong.Play();
            }
        }

        public void SnowballsUpdate(ref Snowball[] snowballs, ref List<MonstreRampant> rampants, ref List<MonstreVolant> volants, TiledMapTileLayer ground)
        {
            if (snowballs.Length > 0)
            {
                SnowballWithMap(ref snowballs, ground);
                SnowballWithMonster(ref snowballs, ref rampants);
                SnowballWithMonster(ref snowballs, ref volants);
                for (int i = 0; i < snowballs.Length; i++)
                {
                    snowballs[i].Move();
                }
            }
        }
        public void SnowballWithMap(ref Snowball[] snowballs, TiledMapTileLayer ground)
        {
            int snowballNull = 0;
            for (int i = 0; i < snowballs.Length; i++)
            {
                // Vérification des collisions avec le décor et la distance
                if (snowballs[i] != null)
                {
                    if (Collision.MapCollision(new Point((int)snowballs[i].Middle.X, (int)snowballs[i].Middle.Y), ground) || snowballs[i].Distance >= 500)
                    {
                        snowballs[i] = null;
                        snowballNull++;
                    }
                    else
                    {
                        snowballs[i].Move();
                    }
                }
            }

            snowballs = UpdateTab(snowballs, snowballNull);
        }
        public void SnowballWithMonster(ref Snowball[] snowballs, ref List<MonstreRampant> rampants)
        {
            int snowballNull = 0;
            List<MonstreRampant> newRampants = new List<MonstreRampant>();
            for (int i = 0; i < rampants.Count; i++)
            {
                bool collide = false;
                for (int j = 0; j < snowballs.Length; j++)
                {
                    if (snowballs[j] != null && (Collision.SpriteCollision(rampants[i].RectangleSprite, snowballs[j].HitBox) || Collision.SpriteCollision(rampants[i].RectangleKill, snowballs[j].HitBox)))
                    {
                        snowballs[j] = null;
                        snowballNull++;
                        collide = true;
                        break;
                    }
                }
                if (!collide)
                {
                    newRampants.Add(rampants[i]);
                }
            }
            snowballs = UpdateTab(snowballs, snowballNull);
            rampants = newRampants;
        }
        public void SnowballWithMonster(ref Snowball[] snowballs, ref List<MonstreVolant> volants)
        {
            int snowballNull = 0;
            List<MonstreVolant> newVolants = new List<MonstreVolant>();
            for (int i = 0; i < volants.Count; i++)
            {
                bool collide = false;
                for (int j = 0; j < snowballs.Length; j++)
                {
                    if (snowballs[j] != null && (Collision.SpriteCollision(volants[i].RectangleSprite, snowballs[j].HitBox) || Collision.SpriteCollision(volants[i].RectangleKill, snowballs[j].HitBox)))
                    {
                        snowballs[j] = null;
                        snowballNull++;
                        collide = true;
                        break;
                    }
                }
                if (!collide)
                {
                    newVolants.Add(volants[i]);
                }
            }
            snowballs = UpdateTab(snowballs, snowballNull);
            volants = newVolants;
        }
        public Snowball[] UpdateTab(Snowball[] tab, int count)
        {
            Snowball[] newTab = new Snowball[tab.Length - count];
            int index = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] != null)
                {
                    newTab[index] = tab[i];
                    index++;
                }
            }

            return newTab;
        }
    }
}
