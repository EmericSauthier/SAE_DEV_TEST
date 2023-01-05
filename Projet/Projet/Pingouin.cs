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
    internal class Pingouin
    {
        private Vector2 position;
        private AnimatedSprite perso;
        private double vitesseMarche;
        private double vitesseSlide;

        private bool slide;
        private bool fly;

        public Pingouin(float x, float y)
        {
            this.Position = new Vector2(x, y);
            this.slide = false;
            this.vitesseMarche = 1;
            this.vitesseSlide = 1.5;
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
        public AnimatedSprite Perso
        {
            get
            {
                return this.perso;
            }

            set
            {
                this.perso = value;
            }
        }
        public double VitesseMarche
        {
            get
            {
                return this.vitesseMarche;
            }

            set
            {
                this.vitesseMarche = value;
            }
        }
        public double VitesseSlide
        {
            get
            {
                return this.vitesseSlide;
            }

            set
            {
                this.vitesseSlide = value;
            }
        }

        public Vector2 Animate(bool gameOver, KeyboardState keyboardState)
        {
            Vector2 move = Vector2.Zero;
            if (gameOver)
            {
                this.perso.Play("celebrate");
            }
            else if (keyboardState.IsKeyDown(Keys.Space))
            {
                this.slide = false;
                float direction = 0;
                if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
                {
                    direction = (float)(vitesseMarche / 2);
                }
                Jump(direction);
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                //if (!IsCollision(x,y, out tile))
                this.slide = false;
                this.perso.Play("walkForward");
                move = new Vector2((float)VitesseMarche, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right))
            {
                this.slide = false;
                this.perso.Play("walkBehind");
                move = new Vector2((float)-vitesseMarche, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                if (!this.slide)
                {
                    this.perso.Play("beforeSlide");
                    this.slide = true;
                }
                else
                {
                    this.perso.Play("slide");
                    move = new Vector2((float)vitesseSlide, 0);
                }
            }
            else
            {
                this.slide = false;
                this.perso.Play("idle");
            }
            this.position += move;
            return move;
        }

        public Vector2 Jump(float direction)
        {
            Vector2 move = Vector2.Zero;
            if (!fly)
            {
                this.perso.Play("beforeJump");
                move = new Vector2(direction, -5);
                fly = true;
            }
            else
            {
                this.perso.Play("jump");
            }
            return move;
        }
    }
}
