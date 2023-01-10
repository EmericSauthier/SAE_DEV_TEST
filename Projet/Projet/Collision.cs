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
                if((monstre.IsMovingRight && pingouin.IsMovingRight) || (!monstre.IsMovingRight && pingouin.IsMovingRight))
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

        public bool SpriteCollision(RectangleF rect1, RectangleF rect2)
        {
            return rect1.Intersects(rect2);
        }

        public bool MapCollision(Point[] pointTab, TiledMapTileLayer mapLayer)
        {
            TiledMapTile? tile;

            for (int i = 0; i < pointTab.Length; i++)
            {
                if (mapLayer.TryGetTile((ushort)pointTab[i].X, (ushort)pointTab[i].Y, out tile) != false && !tile.Value.IsBlank)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
