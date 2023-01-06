using Microsoft.Xna.Framework;
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
        public static bool IsCollidingTrap(Pingouin pingouin, int largeurPingouin, int hauteurPingouin, Trap trap, int largeurTrap, int hauteurTrap, float scale, bool canCollidingtrap)
        {
            if (canCollidingtrap)
            {
                Rectangle _hitBoxTrap = new Rectangle((int)trap.Position.X, (int)trap.Position.Y + largeurTrap - 14, (int)(largeurTrap * scale), (int)(14 * scale));
                Rectangle _hitBoxPingouin = new Rectangle((int)pingouin.Position.X - 10, (int)pingouin.Position.Y, (int)(largeurPingouin * scale), (int)(hauteurPingouin * scale));

                if (_hitBoxPingouin.Intersects(_hitBoxTrap))
                {
                    return true;
                }
                else return false;
            }
            else return false;

        }

        // Nécessite changement fonctionnement des offsets
        public static bool IsCollidingMonstreRampant(Pingouin pingouin, int largeurPingouin, int hauteurPingouin, MonstreRampant monstre, int largeurMonstre, int hauteurMonstre , float scale)
        {
            Rectangle _hitBoxMonstre = new Rectangle((int)monstre.Position.X, (int)monstre.Position.Y + hauteurMonstre, (int)(largeurMonstre * scale), (int)(hauteurMonstre * scale));
            Rectangle _hitBoxPingouin = new Rectangle((int)pingouin.Position.X, (int)pingouin.Position.Y, (int)(largeurPingouin * scale), (int)(hauteurPingouin * scale));

            if (_hitBoxPingouin.Intersects(_hitBoxMonstre))
            {
                return true;
            }
            else return false;

        }

        public static bool IsCollidingRecompense(Pingouin pingouin, int largeurPingouin, int hauteurPingouin, Recompenses recompense, int largeurRecompense, int hauteurRecompense, float scale)
        {
            Rectangle _hitBoxRecompense = new Rectangle((int)recompense.Position.X, (int)recompense.Position.Y, (int)(largeurRecompense * scale), (int)(hauteurRecompense * scale));
            Rectangle _hitBoxPingouin = new Rectangle((int)pingouin.Position.X, (int)pingouin.Position.Y, (int)(largeurPingouin * scale), (int)(hauteurPingouin * scale));

            if (_hitBoxPingouin.Intersects(_hitBoxRecompense))
            {
                return true;
            }
            else return false;

        }
    }
}
