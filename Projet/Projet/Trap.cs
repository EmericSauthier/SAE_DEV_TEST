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
        private double chronoActivation;

        public Trap(Vector2 trapPosition, string trapTypeString)
        {
            this.Position = trapPosition;
            this.TrapType = trapTypeString;
            this.CanCollidingTrap = true;
            this.ChronoActivation = 0;

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

        public double ChronoActivation
        {
            get
            {
                return this.chronoActivation;
            }

            set
            {
                this.chronoActivation = value;
            }
        }

        public void LoadContent(SpriteSheet sprite)
        {
            Sprite = new AnimatedSprite(sprite);
        }

        public void PressActivation(GameTime gameTime)
        {
            ChronoActivation += (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.CanCollidingTrap = false;
            if (ChronoActivation > 1 && ChronoActivation < 1.3)
            {
                Sprite.Play("press");
            }
            else if(ChronoActivation >= 1.5&& ChronoActivation < 2)
            {
                this.CanCollidingTrap = true;
            }
            else if(ChronoActivation >= 2 && ChronoActivation < 4)  
            {
                this.CanCollidingTrap = false;
            }
            else if(ChronoActivation >= 4)
            {
                ChronoActivation = 0;
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
