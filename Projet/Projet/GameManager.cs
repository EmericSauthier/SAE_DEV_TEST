using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

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

        public GameManager()
        {
            _timer = 0;
            _timerSpike = 0;
        }

        public void Update(Game1 game, KeyboardState keyboardState, Pingouin pingouin, ref Snowball[] snowballs, TiledMap map, float deltaTime, TiledMapTileLayer spikes = null)
        {
            TiledMapTileLayer ground = map.GetLayer<TiledMapTileLayer>("Ground");
            TiledMapTileLayer dead = map.GetLayer<TiledMapTileLayer>("DeadZone");

            if (pingouin.CurrentLife <= 0 || Collision.MapCollision(pingouin.CheckBottom(), dead))
            {
                game.clicDead = true;
            }
            else
            {
                _timer += deltaTime;
                _timerSpike += deltaTime;
                InputsManager(keyboardState, pingouin, ref snowballs, ground);
                SnowballsUpdate(ref snowballs, ground);
                pingouin.Update(deltaTime, ground);
                if (spikes != null && Collision.MapCollision(new Point((int)pingouin.Position.X, (int)(pingouin.Position.Y + 50 * pingouin.Scale)), spikes) && _timerSpike >= 2)
                {
                    pingouin.CurrentLife -= 1;
                    _timerSpike = 0;
                }
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
                System.Diagnostics.Debug.WriteLine("Right : " + pingouin.IsMovingRight + "\nLeft : " + pingouin.IsMovingLeft);
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
        public void SnowballsUpdate(ref Snowball[] snowballs, TiledMapTileLayer mapLayer)
        {
            int countNull = 0;
            for (int i = 0; i < snowballs.Length; i++)
            {
                // Vérification des collisions avec le décor et la distance
                if (snowballs[i].Collide(mapLayer) || snowballs[i].Distance >= 500)
                {
                    snowballs[i] = null;
                    countNull++;
                }
                else
                {
                    snowballs[i].Move();
                }
            }

            Snowball[] temp = snowballs;
            snowballs = new Snowball[snowballs.Length - countNull];
            int index = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != null)
                {
                    snowballs[index] = temp[i];
                    index++;
                }
            }
        }
    }
}
