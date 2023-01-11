using Microsoft.Xna.Framework;

using MonoGame.Extended.Sprites;

namespace Projet
{
    internal class Recompenses
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private string typeRecompense;
        public int etat;
        private int hauteur;

        public static int effetPiece = -1;
        private int largeur;
        private Rectangle rectangleSprite;

        public Recompenses(Vector2 position, string typeRecompense, int etat)
        {
            this.Position = position;
            this.Sprite = sprite;
            this.TypeRecompense = typeRecompense;
            UpdateDimensions();
            UpdateBoxes();
            this.etat = etat;
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

        public string TypeRecompense
        {
            get
            {
                return this.typeRecompense;
            }

            set
            {
                this.typeRecompense = value;
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

        public void LoadContent(SpriteSheet sprite)
        {
            Sprite = new AnimatedSprite(sprite);
        }

        public void UpdateBoxes()
        {
            this.RectangleSprite = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(this.Largeur), (int)(this.Hauteur));
            
        }

        public void UpdateDimensions()
        {
            if(this.TypeRecompense == "piece")
            {
                this.Largeur = 10;
                this.Hauteur = 10;
            }
            else if(this.TypeRecompense == "portal")
            {
                this.Largeur = 10;
                this.Hauteur = 10;
            }
        }

    }
}
