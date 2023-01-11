using Microsoft.Xna.Framework;

using MonoGame.Extended.Sprites;

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
        private bool hasLostPlayer;
        private double chronoDep;
        private Vector2 positionDeBase;
        private bool hasTouchPlayer;

        private int largeur, hauteur;
        private Rectangle rectangleSprite, rectangleKill, rectangleDetection;

        public MonstreVolant(Vector2 position, string enemy, double vitesse, double tempsArrivePosition)
        {
            this.Position = position;
            this.PositionDeBase = position;
            this.Enemy = enemy;
            this.Vitesse = vitesse;
            this.TempsArrivePosition = tempsArrivePosition;
            vitessePoursuite = this.Vitesse * 1.3;
            IsDied = false;
            this.ChronoDep = 0;
            this.HasSawPlayer = false;
            this.HasTouchPlayer = false;

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

        public Vector2 PositionDeBase
        {
            get
            {
                return this.positionDeBase;
            }

            set
            {
                this.positionDeBase = value;
            }
        }

        public bool HasTouchPlayer
        {
            get
            {
                return this.hasTouchPlayer;
            }

            set
            {
                this.hasTouchPlayer = value;
            }
        }

        public void Move(GameTime gameTime, Pingouin pingouin)
        {
            ChronoDep += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (HasTouchPlayer)
            {
                ReturnToBasePos();
            }
            else
            {
                if (HasSawPlayer)
                {
                    ChaseEnemy(pingouin);
                }
                else
                {
                    IdleFlying();
                }
            }
        }

        public void IdleFlying()
        {
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

        public void ReturnToBasePos()
        {
            if (this.position.X > PositionDeBase.X)
            {
                Sprite.Play("flyLeft");
                this.position.X -= (float)vitessePoursuite;
            }
            else
            {
                Sprite.Play("flyRight");
                this.position.X += (float)vitessePoursuite;
            }

            if (this.position.Y > PositionDeBase.Y)
            {
                this.position.Y -= (float)vitessePoursuite;
            }
            else
            {
                Sprite.Play("flyRight");
                this.position.Y += (float)vitessePoursuite;
            }

            if ((int)PositionDeBase.X < (int)this.Position.X + 20 && (int)PositionDeBase.X > (int)this.Position.X - 20)
            {
                if ((int)PositionDeBase.Y < (int)this.Position.Y + 20 && (int)PositionDeBase.Y > (int)this.Position.Y - 20)
                {
                    this.HasTouchPlayer = false;
                    this.hasLostPlayer = false;
                }
            }
        }

        public void ChaseEnemy(Pingouin pingouin)
        {
            if (this.position.X > pingouin.Position.X)
            {
                Sprite.Play("flyLeft");
                this.position.X -= (float)vitessePoursuite;
            }
            else if ((int)pingouin.Position.X < (int)this.Position.X + 20 && (int)pingouin.Position.X > (int)this.Position.X - 20)
            {
                if((int)pingouin.Position.Y < (int)this.Position.Y)
                {
                    this.Sprite.Play("flyTop");
                }else
                {
                    this.Sprite.Play("flyBottom");
                }

            }
            else 
            {
                Sprite.Play("flyRight");
                this.position.X += (float)vitessePoursuite;
            }

            if (this.position.Y > pingouin.Position.Y)
            {
                this.position.Y -= (float)Vitesse;
            }
            else
            {
                this.position.Y += (float)Vitesse;
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
                //game.SpriteBatch.DrawRectangle(this.RectangleSprite, Color.Green);
                //game.SpriteBatch.DrawRectangle(this.RectangleKill, Color.DarkGreen);
                //game.SpriteBatch.DrawRectangle(this.RectangleDetection, Color.DarkGreen);
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
