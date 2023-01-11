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
    internal class MonstreVolant
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private string enemy;
        private double vitesse;
        private double vitessePoursuite;
        private bool isMovingRight; //ajout isMovingUp pour collision verticale ?
        private double tempsArrivePosition;
        private bool isDied;
        private bool hasSawPlayer;
        private double chronoDep;

        private int largeur, hauteur;
        private Rectangle rectangleSprite, rectangleKill, rectangleDetection;

        public MonstreVolant(Vector2 position, string enemy, double vitesse, double tempsArrivePosition)
        {
            this.Position = position;
            this.Enemy = enemy;
            this.Vitesse = vitesse;
            this.TempsArrivePosition = tempsArrivePosition;
            vitessePoursuite = this.Vitesse * 1.3;
            IsDied = false;
            this.ChronoDep = 0;

            UpdateDimensions();
            UpdateBoxes();
        }

        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }

        public AnimatedSprite Sprite
        {
            get
            {
                return this.sprite;
            }

            set
            {
                this.sprite = value;
            }
        }

        public string Enemy
        {
            get
            {
                return this.enemy;
            }

            set
            {
                this.enemy = value;
            }
        }

        public double Vitesse
        {
            get
            {
                return this.vitesse;
            }

            set
            {
                this.vitesse = value;
            }
        }

        public bool IsMovingRight
        {
            get
            {
                return this.isMovingRight;
            }

            set
            {
                this.isMovingRight = value;
            }
        }

        public double TempsArrivePosition
        {
            get
            {
                return this.tempsArrivePosition;
            }

            set
            {
                this.tempsArrivePosition = value;
            }
        }

        public bool IsDied
        {
            get
            {
                return this.isDied;
            }

            set
            {
                this.isDied = value;
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

        public Rectangle RectangleKill
        {
            get
            {
                return this.rectangleKill;
            }

            set
            {
                this.rectangleKill = value;
            }
        }

        public Rectangle RectangleDetection
        {
            get
            {
                return this.rectangleDetection;
            }

            set
            {
                this.rectangleDetection = value;
            }
        }

        public bool HasSawPlayer
        {
            get
            {
                return this.hasSawPlayer;
            }

            set
            {
                this.hasSawPlayer = value;
            }
        }

        public double ChronoDep
        {
            get
            {
                return this.chronoDep;
            }

            set
            {
                this.chronoDep = value;
            }
        }

        public void Move(GameTime gameTime, Pingouin pingouin)
        {
            ChronoDep += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!HasSawPlayer)
            {
                //System.Diagnostics.Debug.WriteLine(time);
                if (ChronoDep <= tempsArrivePosition)
                {
                    Position += new Vector2((float)Vitesse, 0);
                    Sprite.Play("flyRight");
                    IsMovingRight = true;
                }
                else if (ChronoDep > tempsArrivePosition && ChronoDep < tempsArrivePosition * 2)
                {
                    Position -= new Vector2((float)Vitesse, 0);
                    Sprite.Play("flyLeft");
                    IsMovingRight = false;
                }
                else ChronoDep = 0;
            }
            else
            {
                if(this.position.X > pingouin.Position.X)
                {
                    Sprite.Play("flyLeft");
                    this.position.X -= (float)vitessePoursuite;
                }
                else
                {
                    Sprite.Play("flyRight");
                    this.position.X += (float)vitessePoursuite;
                }

                if(this.position.Y > pingouin.Position.Y)
                {
                    this.position.Y -= (float)Vitesse;
                }else
                {
                    this.position.Y += (float)Vitesse;
                }
            }

        }

        public void LoadContent(SpriteSheet sprite)
        {
            Sprite = new AnimatedSprite(sprite);
        }

        public void Affiche(Game1 game)
        {
            if (!IsDied)
            {
                game.SpriteBatch.Draw(this.Sprite, this.Position, 0, new Vector2(2, 2));
                // DEBUG
                game.SpriteBatch.DrawRectangle(this.RectangleSprite, Color.Green);
                game.SpriteBatch.DrawRectangle(this.RectangleKill, Color.DarkGreen);
                game.SpriteBatch.DrawRectangle(this.RectangleDetection, Color.DarkGreen);
            }
        }
        public void UpdateBoxes()
        {
            if (enemy == "eagle")
            {
                this.RectangleSprite = new Rectangle((int)this.Position.X - 30, (int)this.Position.Y - 12, (int)(this.Largeur), (int)(this.Hauteur));
                this.RectangleKill = new Rectangle((int)this.Position.X - 22, (int)this.Position.Y - 20, (int)(this.Largeur) - 16, 10);
                this.RectangleDetection = new Rectangle((int)(this.Position.X - 200), (int)(this.Position.Y - 150), this.Largeur * 10, this.Hauteur*10);
            }
        }
        public void UpdateDimensions()
        {
            if (enemy == "eagle")
            {
                this.Largeur = 50;
                this.Hauteur = 30;
            }
        }

    }
}
