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
        private Sprite[] hearts;
        private Vector2[] heartPosition;
        private int currentLife;
        private int maxLife;

        public Life(int maxLife)
        {
            this.CurrentLife = maxLife;
            this.MaxLife = maxLife;
            this.Hearts = new Sprite[maxLife];
            this.HeartsPosition = new Vector2[maxLife];
        }

        public Sprite[] Hearts
        {
            get
            {
                return this.hearts;
            }

            set
            {
                this.hearts = value;
            }
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

        public Vector2[] HeartsPosition
        {
            get
            {
                return this.heartPosition;
            }

            set
            {
                this.heartPosition = value;
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < HeartsPosition.Length; i++)
            {
                HeartsPosition[i] = new Vector2(100*i, 100);
            }
        }

        public void Update()
        {
            for (int i = 0; i < Hearts.Length; i++)
            {

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Hearts.Length; i++)
            {
                spriteBatch.Draw(Hearts[i], HeartsPosition[i], 0f, new Vector2(2, 2));
            }
        }

        public void TakeDamage(int damage, float invincibilityChrono)
        {
            if (invincibilityChrono > 2)
            {
                CurrentLife -= damage;
            }
            invincibilityChrono = 0;
        }
    }
}
