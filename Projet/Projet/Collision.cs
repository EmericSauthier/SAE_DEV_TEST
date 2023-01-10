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
        public static bool IsCollidingTrap(Trap trap, bool canCollidingtrap, ref Rectangle rectangleTrapDebug, Rectangle hitboxPingouin)
        {
            if (canCollidingtrap)
            {
                rectangleTrapDebug = new Rectangle((int)trap.Position.X - 15, (int)trap.Position.Y - 8, trap.Largeur , trap.Hauteur);

                if (hitboxPingouin.Intersects(rectangleTrapDebug))
                {
                    return true;
                }
                else return false;
            }
            else return false;

        }

        // Nécessite changement fonctionnement des offsets
        public static bool IsCollidingMonstre(Pingouin pingouin, MonstreRampant monstre, ref Rectangle rectangleMonstreDebug, ref Rectangle rectangleKillingMonster, Rectangle hitboxPingouin)
        {
            rectangleMonstreDebug = new Rectangle((int)monstre.Position.X - 30, (int)monstre.Position.Y, (int)(monstre.Largeur), (int)(monstre.Hauteur));
            rectangleKillingMonster = new Rectangle((int)monstre.Position.X - 22, (int)monstre.Position.Y - 10, (int)(monstre.Largeur)-16, 10);
            

            if (hitboxPingouin.Intersects(rectangleKillingMonster))
            {
                monstre.IsDied = true;
            }

            if (hitboxPingouin.Intersects(rectangleMonstreDebug))
            {
                if((monstre.IsMovingRight && pingouin.isMovingRight) || (!monstre.IsMovingRight && pingouin.isMovingRight))
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

        public static bool IsCollidingMonstre(Pingouin pingouin, MonstreVolant monstre, ref Rectangle rectangleMonstreDebug, ref Rectangle rectangleKillingMonster, Rectangle hitboxPingouin)
        {
            rectangleMonstreDebug = new Rectangle((int)monstre.Position.X - 30, (int)monstre.Position.Y - 12, (int)(monstre.Largeur), (int)(monstre.Hauteur));
            rectangleKillingMonster = new Rectangle((int)monstre.Position.X - 22, (int)monstre.Position.Y - 20, (int)(monstre.Largeur) - 16, 10);


            if (hitboxPingouin.Intersects(rectangleKillingMonster))
            {
                monstre.IsDied = true;
            }

            if (hitboxPingouin.Intersects(rectangleMonstreDebug))
            {
                if ((monstre.IsMovingRight && pingouin.isMovingRight) || (!monstre.IsMovingRight && pingouin.isMovingRight))
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

        public static bool IsCollidingRecompense(Recompenses recompense, int largeurRecompense, int hauteurRecompense, ref Rectangle recompenseRectangle, Rectangle hitboxPingouin)
        {
            recompenseRectangle = new Rectangle((int)recompense.Position.X, (int)recompense.Position.Y, (int)(largeurRecompense), (int)(hauteurRecompense));

            if (hitboxPingouin.Intersects(recompenseRectangle))
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
