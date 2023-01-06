using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet
{
    internal class Life
    {
        private int currentLife;
        private int maxLife;

        public Life(int maxLife)
        {
            this.CurrentLife = maxLife;
            this.MaxLife = maxLife;
        }

        public int CurrentLife
        {
            get
            {
                return this.currentLife;
            }

            set
            {
                this.currentLife = value;
            }
        }

        public int MaxLife
        {
            get
            {
                return this.maxLife;
            }

            set
            {
                this.maxLife = value;
            }
        }

        public void TakeDamage(int damage, ref float invincibilityChrono)
        {
            if (invincibilityChrono > 2)
            {
                CurrentLife -= damage;
            }
            invincibilityChrono = 0;
        }
    }
}
