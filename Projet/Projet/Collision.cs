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
        public static bool IsCollidingTrap(Trap trap, Pingouin pingouin, float scale, bool canCollidingtrap)
        {
            if (canCollidingtrap)
            {
                Rectangle _hitBoxTrap = new Rectangle((int)trap.Position.X, (int)trap.Position.Y + 50, (int)(64 * scale), (int)(14 * scale));
                Rectangle _hitBoxPingouin = new Rectangle((int)pingouin.Position.X, (int)pingouin.Position.Y, (int)(128 * scale), (int)(128 * scale));

                if (_hitBoxPingouin.Intersects(_hitBoxTrap))
                {
                    return true;
                }
                else return false;
            }
            else return false;

        }
    }
}
