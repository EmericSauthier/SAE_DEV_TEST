using Microsoft.Xna.Framework;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace Projet
{
    internal class Collision
    {
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
