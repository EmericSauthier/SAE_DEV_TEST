using Microsoft.Xna.Framework;

using MonoGame.Extended.Sprites;
using Microsoft.Xna.Framework.Audio;

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
        private SoundEffect deathSong;

        private double chronoDep;
        private int largeur, hauteur;
        private Rectangle rectangleSprite, rectangleKill;

        public MonstreRampant(Vector2 position, string enemy, double vitesse, double tempsArrivePosition)
        {
            this.Position = position;
            this.Vitesse = vitesse;
            this.Enemy = enemy;
            this.TempsArrivePosition = tempsArrivePosition;
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

        public SoundEffect DeathSong
        {
            get
            {
                return this.deathSong;
            }

            set
            {
                this.deathSong = value;
            }
        }

        public void RightLeftMove(GameTime gameTime)
        {
            ChronoDep += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (ChronoDep <= this.TempsArrivePosition)
            {
                Position += new Vector2((float)Vitesse, 0);
                Sprite.Play("rightWalking");
                IsMovingRight = true;
            }
            else if (ChronoDep <= (this.TempsArrivePosition*2))
            {
                Position -= new Vector2((float)Vitesse, 0);
                Sprite.Play("leftWalking");
                IsMovingRight = false;
            }
            else ChronoDep = 0;
        }

        public void LoadContent(SpriteSheet sprite, SoundEffect song)
        {           
            this.Sprite = new AnimatedSprite(sprite);
            this.DeathSong = song;
        }

        public void Affiche(Game1 game)
        {
            if (!IsDied)
            {
                game.SpriteBatch.Draw(this.Sprite, this.Position, 0, new Vector2(3, 3));
                // DEBUG
                //game.SpriteBatch.DrawRectangle(this.RectangleSprite, Color.Green);
                //game.SpriteBatch.DrawRectangle(this.RectangleKill, Color.DarkGreen);
            }
        }

        public void UpdateBoxes()
        {
            if (enemy == "fox")
            {
                this.RectangleSprite = new Rectangle((int)this.Position.X - 30, (int)this.Position.Y, (int)(this.Largeur), (int)(this.Hauteur));
                this.RectangleKill = new Rectangle((int)this.Position.X - 22, (int)this.Position.Y - 10, (int)(this.Largeur) - 16, 10);
            }
        }

        public void UpdateDimensions()
        { 
            if(enemy == "fox")
            {
                this.Largeur = 19 * 3;
                this.Hauteur = 14 * 3;
            }
        }
    }
}
