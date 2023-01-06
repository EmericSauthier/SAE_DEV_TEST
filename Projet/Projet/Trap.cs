using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet
{
    internal class Trap
    {
        private Vector2 _position;
        private AnimatedSprite _sprite;

        public Trap(Vector2 trapPosition)
        {
            this.Position = trapPosition;
        }

        public Vector2 Position
        {
            get
            {
                return this._position;
            }

            set
            {
                this._position = value;
            }
        }

        public AnimatedSprite Sprite
        {
            get
            {
                return this._sprite;
            }

            set
            {
                this._sprite = value;
            }
        }

        public void LoadContent(SpriteSheet sprite)
        {
            Sprite = new AnimatedSprite(sprite);
        }

        public void PressActivation(ref float time, ref bool canCollidingTrap)
        {
            //System.Diagnostics.Debug.WriteLine(time);
            canCollidingTrap = false;
            if (time > 1 && time < 1.3)
            {
                Sprite.Play("press");
            }
            else if(time >= 1.5&& time < 2)
            {
                canCollidingTrap = true;
            }
            else if(time >= 2 && time < 4)  
            {
                canCollidingTrap = false;
            }
            else if(time >= 4)
            {
                time = 0;
            }
        }

        public static void CollisionUpdate()
        {

        }
    }
}
