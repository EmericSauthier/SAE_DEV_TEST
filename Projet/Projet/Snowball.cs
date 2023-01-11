using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended;

namespace Projet
{
    internal class Snowball
    {
        private Texture2D texture;

        private Vector2 position;
        private Vector2 middle;
        private Vector2 velocity;

        private RectangleF hitBox;

        private float distance;
        private int width;
        private int height;

        public Snowball(float x, float y, float scale, Texture2D texture)
        {
            this.Middle = new Vector2(x, y);
            this.Position = this.middle - new Vector2((float)texture.Width / 2, (float)texture.Height / 2) * scale;

            this.Texture = texture;
            this.hitBox = new RectangleF(this.position.X, this.position.Y, texture.Height / 2, texture.Height / 2);

            this.velocity = new Vector2(5, 0);
            this.Distance = 0;

            this.width = (int)(texture.Width * scale);
            this.height = (int)(texture.Height * scale);
        }

        public Texture2D Texture
        {
            get
            {
                return this.texture;
            }

            set
            {
                this.texture = value;
            }
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
        public Vector2 Velocity
        {
            get
            {
                return this.velocity;
            }

            set
            {
                this.velocity = value;
            }
        }
        public Vector2 Middle
        {
            get
            {
                return this.middle;
            }

            set
            {
                this.middle = value;
            }
        }
        public RectangleF HitBox
        {
            get
            {
                return this.hitBox;
            }

            set
            {
                this.hitBox = value;
            }
        }
        public float Distance
        {
            get
            {
                return this.distance;
            }

            set
            {
                this.distance = value;
            }
        }

        public void Affiche(Game1 game)
        {
            Rectangle destination = new Rectangle((int)(this.position.X), (int)(this.position.Y), this.width, this.height);
            game.SpriteBatch.Draw(this.texture, destination, Color.White);
        }
        public void Move()
        {
            this.position += this.velocity;
            this.middle += this.velocity;
            this.hitBox.Position += this.velocity;
            distance += this.velocity.X;
        }
    }
}
