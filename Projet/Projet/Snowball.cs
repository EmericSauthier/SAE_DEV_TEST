using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

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

            this.velocity = new Vector2(2, 0);
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
        public bool Collide(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction vérifiant la collision de la boule de neige avec le décor
            */

            // Définition de deux points et de deux tiles, en bas à gauche et en bas à droite
            ushort x = (ushort)(this.middle.X / mapLayer.TileWidth);
            ushort y = (ushort)(this.middle.Y / mapLayer.TileHeight);

            TiledMapTile? tile;

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            if (mapLayer.TryGetTile(x, y, out tile) != false && !tile.Value.IsBlank)
                return true;

            return false;
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
