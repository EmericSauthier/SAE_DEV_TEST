using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet
{
    internal class AnimatedPress
    {
        private Vector2 _position;
        private AnimatedSprite _sprite;

        public AnimatedPress(Vector2 trapPosition)
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

        /*public void Activation(ref float time)
        {
            if(time > 1 && time < 1.1 && Game1.canCollidingTrap)
            {
                Sprite.Play("press");
            }
        }*/
    }
}
