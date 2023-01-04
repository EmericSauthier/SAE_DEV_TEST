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

        public Penguin(float x, float y)
        {
            this.Position = new Vector2(x, y);
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
                perso.Play("celebrate");
            }
            else if (keyboardState.IsKeyDown(Keys.Down) && !keyboardState.IsKeyDown(Keys.Space))
            {
                perso.Play("beforeSlide");
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space))
            {
                perso.Play("jump");
                position += new Vector2((float)0.5, 0);
            }
            else if (!keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Space))
            {
                perso.Play("jump");
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                perso.Play("walkForward");
                position += new Vector2(1, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right))
            {
                perso.Play("walkBehind");
                position -= new Vector2(1, 0);
            }
            else
            {

            }
        }
    }
}
