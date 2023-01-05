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
        private AnimatedSprite monsterSprite;
        private string enemy;
        private double vitesse;
        private double tempsArrivePosition;

        public MonstreRampant(Vector2 position, string enemy, double vitesse, double tempsArrivePosition)
        {
            this.Position = position;
            this.MonsterSprite = monsterSprite;
            this.Vitesse = vitesse;
            this.TempsArrivePosition = tempsArrivePosition;
        }

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

        public AnimatedSprite MonsterSprite
        {
            get
            {
                return this.monsterSprite;
            }

            set
            {
                this.monsterSprite = value;
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

        public void RightLeftMove(ref float time)
        {
            if (time <= tempsArrivePosition)
            {
                Position += new Vector2((float)Vitesse, 0);
                MonsterSprite.Play("rightWalking");
            }
            else if (time > tempsArrivePosition && time < tempsArrivePosition*2)
            {
                Position -= new Vector2((float)Vitesse, 0);
                MonsterSprite.Play("leftWalking");
            }
            else time = 0;
        }

        

        public void LoadContent(SpriteSheet sprite)
        {           
            MonsterSprite = new AnimatedSprite(sprite);
        }

        public void Matrice()
        {

        }
    }
}
