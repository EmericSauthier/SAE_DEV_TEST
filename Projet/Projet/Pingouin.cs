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
        private bool slide;

        public Pingouin(float x, float y)
        {
            this.Position = new Vector2(x, y);
            this.slide = false;
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

        public void Animate(bool gameOver, KeyboardState keyboardState)
        {
            if (gameOver)
            {
                this.perso.Play("celebrate");
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (!this.slide)
                    {
                        this.perso.Play("beforeSlide");
                        this.slide = true;
                    }
                    else
                    {
                        this.perso.Play("slide");
                        this.position += new Vector2((float)1.5, 0);
                    }
                }
                else if (keyboardState.IsKeyDown(Keys.Space))
                {
                    this.slide = false;
                    this.perso.Play("jump");
                    this.position += new Vector2((float)0.5, 0);
                }
                else
                {
                    this.slide = false;
                    this.perso.Play("walkForward");
                    this.position += new Vector2(1, 0);
                }
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right))
            {
                this.slide = false;
                this.perso.Play("walkBehind");
                this.position -= new Vector2(1, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Space))
            {
                this.slide = false;
                this.perso.Play("jump");
            }
            else
            {
                if (this.slide)
                {
                    this.perso.Play("afterSlide");
                }
                this.slide = false;
            }
        }
    }
}
