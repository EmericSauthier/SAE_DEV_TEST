using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.TextureAtlases;

namespace Projet
{
    internal class MonstreRampant
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private string enemy;
        private double vitesse;
        private double tempsArrivePosition;
        private bool isMovingRight;
        private bool isDied;

        private int largeur, hauteur;
        private Rectangle rectangleSprite, rectangleKill;

        public MonstreRampant(Vector2 position, string enemy, double vitesse, double tempsArrivePosition, int largeurMonstre, int hauteurMonstre)
        {
            this.Position = position;
            this.Vitesse = vitesse;
            this.Enemy = enemy;
            this.TempsArrivePosition = tempsArrivePosition;
            IsDied = false;

            this.Largeur = largeurMonstre;
            this.Hauteur = hauteurMonstre;
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

        public void RightLeftMove(ref float time)
        {
            //System.Diagnostics.Debug.WriteLine(time);
            if (time <= tempsArrivePosition)
            {
                Position += new Vector2((float)Vitesse, 0);
                Sprite.Play("rightWalking");
                IsMovingRight = true;
            }
            else if (time > tempsArrivePosition && time < tempsArrivePosition*2)
            {
                Position -= new Vector2((float)Vitesse, 0);
                Sprite.Play("leftWalking");
                IsMovingRight = false;
            }
            else time = 0;
        }

        public void LoadContent(SpriteSheet sprite)
        {           
            Sprite = new AnimatedSprite(sprite);
        }

        public void Affiche(Game1 game)
        {
            if (!IsDied)
            {
                game.SpriteBatch.Draw(this.Sprite, this.Position, 0, new Vector2(3, 3));
                // DEBUG
                game.SpriteBatch.DrawRectangle(this.RectangleSprite, Color.Green);
                game.SpriteBatch.DrawRectangle(this.RectangleKill, Color.DarkGreen);
            }
        }

        public void UpdateBoxes()
        {
            if(enemy == "fox")
            {
                this.RectangleSprite = new Rectangle((int)this.Position.X - 30, (int)this.Position.Y, (int)(this.Largeur), (int)(this.Hauteur));
                this.RectangleKill = new Rectangle((int)this.Position.X - 22, (int)this.Position.Y - 10, (int)(this.Largeur) - 16, 10);
            }
        }
    }
}
