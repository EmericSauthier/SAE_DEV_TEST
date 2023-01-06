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
        public static bool IsCollidingTrap(Pingouin pingouin, int largeurPingouin, int hauteurPingouin, Trap trap, int largeurTrap, int hauteurTrap, bool canCollidingtrap, ref Rectangle rectangleTrapDebug, ref Rectangle rectanglePingouinDebug)
        {
            if (canCollidingtrap)
            {
                Rectangle _hitBoxTrap = new Rectangle((int)trap.Position.X - 15, (int)trap.Position.Y - 8, largeurTrap , hauteurTrap);
                Rectangle _hitBoxPingouin = new Rectangle((int)pingouin.Position.X - 25, (int)pingouin.Position.Y - 15, (largeurPingouin), (int)(hauteurPingouin));
                rectangleTrapDebug = _hitBoxTrap;
                rectanglePingouinDebug = _hitBoxPingouin;

                if (_hitBoxPingouin.Intersects(_hitBoxTrap))
                {
                    return true;
                }
                else return false;
            }
            else return false;

        }

        // Nécessite changement fonctionnement des offsets
        public static bool IsCollidingMonstreRampant(Pingouin pingouin, int largeurPingouin, int hauteurPingouin, MonstreRampant monstre, int largeurMonstre, int hauteurMonstre, ref Rectangle rectangleFoxDebug, ref Rectangle rectanglePingouinDebug)
        {
            Rectangle _hitBoxMonstre = new Rectangle((int)monstre.Position.X - 30, (int)monstre.Position.Y, (int)(largeurMonstre), (int)(hauteurMonstre));
            Rectangle _hitBoxPingouin = new Rectangle((int)pingouin.Position.X - 25, (int)pingouin.Position.Y - 15, (int)(largeurPingouin), (int)(hauteurPingouin));
            rectangleFoxDebug = _hitBoxMonstre;
            rectanglePingouinDebug = _hitBoxPingouin;

            if (_hitBoxPingouin.Intersects(_hitBoxMonstre))
            {
                return true;
            }
            else return false;

        }

        public static bool IsCollidingRecompense(Pingouin pingouin, int largeurPingouin, int hauteurPingouin, Recompenses recompense, int largeurRecompense, int hauteurRecompense, float scale)
        {
            Rectangle _hitBoxRecompense = new Rectangle((int)recompense.Position.X, (int)recompense.Position.Y, (int)(largeurRecompense), (int)(hauteurRecompense));
            Rectangle _hitBoxPingouin = new Rectangle((int)pingouin.Position.X, (int)pingouin.Position.Y, (int)(largeurPingouin), (int)(hauteurPingouin));

            if (_hitBoxPingouin.Intersects(_hitBoxRecompense))
            {
                return true;
            }
            else return false;

        }
    }
}
