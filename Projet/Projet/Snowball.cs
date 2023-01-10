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
        private Vector2 velocity;

        public Snowball(float x, float y, Texture2D texture)
        {
            this.Position = new Vector2(x, y);
            this.Texture = texture;
            this.velocity = new Vector2(2, 0);
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

        public bool Collide(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction vérifiant la collision de la boule de neige avec le décor
            */

            // Définition de deux points et de deux tiles, en bas à gauche et en bas à droite
            ushort x = (ushort)(this.Position.X / mapLayer.TileWidth);
            ushort y = (ushort)(this.Position.Y / mapLayer.TileHeight);

            TiledMapTile? tile;

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            if (mapLayer.TryGetTile(x, y, out tile) != false && !tile.Value.IsBlank)
                return true;

            return false;
        }
        public void Move()
        {
            this.position += this.velocity;
        }
    }
}
