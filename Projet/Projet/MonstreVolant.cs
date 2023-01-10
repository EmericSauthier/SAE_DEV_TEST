using Microsoft.Xna.Framework;
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
        private bool isMovingRight;
        private double tempsArrivePosition;
        private bool isDied;

        private int largeur, hauteur;

        public MonstreVolant(Vector2 position, string enemy, double vitesse, double tempsArrivePosition, int largeurMonstre, int hauteurMonstre)
        {
            this.Position = position;
            this.Enemy = enemy;
            this.Vitesse = vitesse;
            this.TempsArrivePosition = tempsArrivePosition;
            vitessePoursuite = this.Vitesse * 1.3;
            IsDied = false;

            this.Largeur = largeurMonstre;
            this.Hauteur = hauteurMonstre;
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

        public void IdleFlying(ref float time)
        {
            //System.Diagnostics.Debug.WriteLine(time);
            if (time <= tempsArrivePosition)
            {
                Position += new Vector2((float)Vitesse, 0);
                Sprite.Play("flyRight");
                IsMovingRight = true;
            }
            else if (time > tempsArrivePosition && time < tempsArrivePosition * 2)
            {
                Position -= new Vector2((float)Vitesse, 0);
                Sprite.Play("flyLeft");
                IsMovingRight = false;
            }
            else time = 0;
        }

        public void LoadContent(SpriteSheet sprite)
        {
            Sprite = new AnimatedSprite(sprite);
        }
    }
}
