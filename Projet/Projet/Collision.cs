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
    internal class Collision
    {
        // Nécessite changement fonctionnement des offsets
        public static bool IsCollidingTrap(Trap trap, Rectangle hitboxPingouin)
        {
            if (trap.CanCollidingTrap)
            {
                if (hitboxPingouin.Intersects(trap.RectangleSprite))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        // Nécessite changement fonctionnement des offsets
        public static bool IsCollidingMonstre(Pingouin pingouin, MonstreRampant monstre, Rectangle hitboxPingouin)
        {
            if (hitboxPingouin.Intersects(monstre.RectangleKill))
            {
                monstre.IsDied = true;
            }

            if (hitboxPingouin.Intersects(monstre.RectangleSprite))
            {
                if((monstre.IsMovingRight && pingouin.IsMovingRight) || (!monstre.IsMovingRight && pingouin.IsMovingRight) || (!monstre.IsMovingRight && !pingouin.IsMovingRight && !pingouin.IsMovingLeft))
                {
                    pingouin.Position -= new Vector2(10, 0);
                }
                else
                {
                    pingouin.Position += new Vector2(10, 0);
                }

                return true;
            }
            else return false;

        }

        public static bool IsCollidingMonstre(Pingouin pingouin, MonstreVolant monstre, Rectangle hitboxPingouin)
        {
            if (hitboxPingouin.Intersects(monstre.RectangleKill))
            {
                monstre.IsDied = true;
            }

            if (hitboxPingouin.Intersects(monstre.RectangleDetection))
            {
                monstre.HasSawPlayer = true;
            }else monstre.HasSawPlayer = false;

            if (hitboxPingouin.Intersects(monstre.RectangleSprite))
            {
                if ((monstre.IsMovingRight && pingouin.IsMovingRight) || (!monstre.IsMovingRight && pingouin.IsMovingRight))
                {
                    pingouin.Position -= new Vector2(10, 0);
                }
                else
                {
                    pingouin.Position += new Vector2(10, 0);
                }

                return true;
            }
            else return false;

        }

        public static bool IsCollidingRecompense(Recompenses recompense, Rectangle hitboxPingouin)
        {
            if (hitboxPingouin.Intersects(recompense.RectangleSprite))
            {
                return true;
            }
            else return false;

        }

        public static bool SpriteCollision(RectangleF rect1, RectangleF rect2)
        {
            return rect1.Intersects(rect2);
        }
        public bool SpriteCollision(RectangleF rect1, RectangleF[] tab)
        {
            bool collision = false;
            for (int i = 0; i < tab.Length; i++)
            {
                collision = collision || rect1.Intersects(tab[i]);
            }
            return collision;
        }

        public static bool MapCollision(Point[] pointTab, TiledMapTileLayer mapLayer)
        {
            for (int i = 0; i < pointTab.Length; i++)
            {
                TiledMapTile? tile;
                if (mapLayer.TryGetTile((ushort)(pointTab[i].X / mapLayer.TileWidth), (ushort)(pointTab[i].Y / mapLayer.TileHeight), out tile) != false && !tile.Value.IsBlank)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool MapCollision(Point point, TiledMapTileLayer mapLayer)
        {
            TiledMapTile? tile;

            if (mapLayer.TryGetTile((ushort)(point.X / mapLayer.TileWidth), (ushort)(point.Y / mapLayer.TileHeight), out tile) != false && !tile.Value.IsBlank)
            {
                return true;
            }
            return false;
        }
    }
}
