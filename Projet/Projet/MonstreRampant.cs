﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Content;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.TextureAtlases;

namespace Projet
{
    internal class MonstreRampant
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private string enemy;
        private double vitesse;
        private double tempsArrivePosition;
        private bool isMovingRight;

        /* CREATION VITESSE POUR TYPE DE RAMPANTS (POSSIBILITE D'AMELIORATION)
         private double _vitesseFox;
        private double _vitesseAutre;
         */

        public MonstreRampant(Vector2 position, string enemy, double vitesse, double tempsArrivePosition)
        {
            this.Position = position;
            this.Vitesse = vitesse;
            this.Enemy = enemy;
            this.TempsArrivePosition = tempsArrivePosition;
        }
        /* POSSIBILITER D'AMELIORATION EN METTANT AUTOMATIQUEMENT LA VITESSE CELON LE TYPE DE RAMPANTS
         public MonstreRampant(Vector2 position, string enemy, double tempsArrivePosition)
        {
            this.Position = position;
            if (ennemy == "fox")
                this.Vitesse = _vitesseFox;
            else
                this.Vitesse = _vitesse;
            this.Enemy = enemy;
            this.TempsArrivePosition = tempsArrivePosition;
        }
         */

        public Vector2 Position
        {
            get
            {
                return this.position;
            }

            set
            {
                this.position = value;
            }
        }
        public AnimatedSprite Sprite
        {
            get
            {
                return this.sprite;
            }

            set
            {
                this.sprite = value;
            }
        }
        public double Vitesse
        {
            get
            {
                return this.vitesse;
            }

            set
            {
                this.vitesse = value;
            }
        }
        public string Enemy
        {
            get
            {
                return this.enemy;
            }

            set
            {
                this.enemy = value;
            }
        }
        public double TempsArrivePosition
        {
            get
            {
                return this.tempsArrivePosition;
            }

            set
            {
                this.tempsArrivePosition = value;
            }
        }

        public bool IsMonsterRight
        {
            get
            {
                return this.isMovingRight;
            }

            set
            {
                this.isMovingRight = value;
            }
        }

        public void RightLeftMove(ref float time)
        {
            //System.Diagnostics.Debug.WriteLine(time);
            if (time <= tempsArrivePosition)
            {
                Position += new Vector2((float)Vitesse, 0);
                Sprite.Play("rightWalking");
                IsMonsterRight = true;
            }
            else if (time > tempsArrivePosition && time < tempsArrivePosition*2)
            {
                Position -= new Vector2((float)Vitesse, 0);
                Sprite.Play("leftWalking");
                IsMonsterRight = false;
            }
            else time = 0;
        }

        public void LoadContent(SpriteSheet sprite)
        {           
            Sprite = new AnimatedSprite(sprite);
        }
    }
}
