﻿using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            rectangleMonstreDebug = new Rectangle((int)monstre.Position.X - 30, (int)monstre.Position.Y - 12, (int)(monstre.LargeurMonstre), (int)(monstre.HauteurMonstre));
            rectangleKillingMonster = new Rectangle((int)monstre.Position.X - 22, (int)monstre.Position.Y - 20, (int)(monstre.LargeurMonstre) - 16, 10);


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
    }
}
