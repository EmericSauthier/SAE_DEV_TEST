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

        private int largeur, hauteur;

        public Trap(Vector2 trapPosition, int largeurTrap, int hauteurTrap)
        {
            this.Position = trapPosition;
            this.Largeur = largeurTrap;
            this.Hauteur = hauteurTrap;
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

        public int Largeur
        {
            get
            {
                return this.largeur;
            }

            set
            {
                this.largeur = value;
            }
        }

        public int Hauteur
        {
            get
            {
                return this.hauteur;
            }

            set
            {
                this.hauteur = value;
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
    }
}
