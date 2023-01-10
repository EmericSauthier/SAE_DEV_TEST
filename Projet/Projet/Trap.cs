using Microsoft.Xna.Framework;
using MonoGame.Extended;
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
        private string trapType;

        private int largeur, hauteur;
        private Rectangle rectangleSprite;
        private bool canCollidingTrap;

        public Trap(Vector2 trapPosition, string trapTypeString)
        {
            this.Position = trapPosition;
            this.TrapType = trapTypeString;
            this.CanCollidingTrap = true;

            UpdateDimensions();
            UpdateBoxes();
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

        public Rectangle RectangleSprite
        {
            get
            {
                return this.rectangleSprite;
            }

            set
            {
                this.rectangleSprite = value;
            }
        }

        public string TrapType
        {
            get
            {
                return this.trapType;
            }

            set
            {
                this.trapType = value;
            }
        }

        public bool CanCollidingTrap
        {
            get
            {
                return this.canCollidingTrap;
            }

            set
            {
                this.canCollidingTrap = value;
            }
        }

        public void LoadContent(SpriteSheet sprite)
        {
            Sprite = new AnimatedSprite(sprite);
        }

        public void PressActivation(ref double time)
        {
            //System.Diagnostics.Debug.WriteLine(time);
            this.CanCollidingTrap = false;
            if (time > 1 && time < 1.3)
            {
                Sprite.Play("press");
            }
            else if(time >= 1.5&& time < 2)
            {
                this.CanCollidingTrap = true;
            }
            else if(time >= 2 && time < 4)  
            {
                this.CanCollidingTrap = false;
            }
            else if(time >= 4)
            {
                time = 0;
            }
        }

        public void UpdateBoxes()
        {
            if(this.TrapType == "press")
            {
                this.RectangleSprite = new Rectangle((int)this.Position.X - 15, (int)this.Position.Y - 8, this.Largeur, this.Hauteur);
            }
        }

        public void UpdateDimensions()
        {
            if(this.TrapType == "press")
            {
                this.Largeur = 64 / 2;
                this.Hauteur = 64 - 20;
            }
        }

        public void Affiche(Game1 game)
        {
            game.SpriteBatch.Draw(this.Sprite, this.Position, 0, new Vector2(1, 1));
            game.SpriteBatch.DrawRectangle(this.RectangleSprite, Color.Orange);
        }
    }
}
