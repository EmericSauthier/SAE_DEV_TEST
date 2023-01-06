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
        private Vector2 positionSaut;

        private AnimatedSprite perso;

        private double walkVelocity;
        private double slideVelocity;
        private double jumpVelocity;
        private double gravity;

        private bool slide;
        private bool fly;
        private bool jump;

        public Pingouin(float x, float y)
        {
            this.Position = new Vector2(x, y);
            this.slide = false;
            this.walkVelocity = 1;
            this.slideVelocity = 1.5;
            this.gravity = 2.5;
            this.jumpVelocity = 10;
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
        public double WalkVelocity
        {
            get
            {
                return this.walkVelocity;
            }

            set
            {
                this.walkVelocity = value;
            }
        }
        public double SlideVelocity
        {
            get
            {
                return this.slideVelocity;
            }

            set
            {
                this.slideVelocity = value;
            }
        }
        public bool Fly
        {
            get
            {
                return this.fly;
            }

            set
            {
                this.fly = value;
            }
        }

        public void Animate(bool gameOver, KeyboardState keyboardState, TiledMapTileLayer mapLayer)
        {
            Gravity(mapLayer);
            Vector2 move = Vector2.Zero;

            // Si le jeu est fini
            if (gameOver)
            {
                this.perso.Play("celebrate");
            }
            else if ((keyboardState.IsKeyDown(Keys.Space) || this.fly))
            {
                this.slide = false;
                Jump(ref move, keyboardState, mapLayer);
            }
            else if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                this.slide = false;
                this.perso.Play("walkForward");
                if (!CheckRight(mapLayer))
                    move = new Vector2((float)walkVelocity, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right))
            {
                this.slide = false;
                this.perso.Play("walkBehind");
                if (!CheckLeft(mapLayer))
                    move = new Vector2((float)-walkVelocity, 0);
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
                    if (!CheckRight(mapLayer))
                        move = new Vector2((float)slideVelocity, 0);
                }
            }
            else
            {
                this.slide = false;
                this.perso.Play("idle");
            }

            this.position += move;
        }
        public void Animate(String animation)
        {
            switch (animation)
            {
                // Joue l'animation de célébration
                case "celebrate":
                    this.perso.Play("celebrate");
                    break;
                // Joue l'animation de déplacement vers la droite
                case "walkForward":
                    this.perso.Play("walkForward");
                    break;
                // Joue l'animation de déplacement vers la gauche
                case "walkBehind":
                    this.perso.Play("walkBehind");
                    break;
                // Joue l'animation de glisse
                case "slide":
                    this.perso.Play("beforeSlide");
                    this.perso.Play("slide");
                    break;
                // Joue l'animation de saut
                case "jump":
                    this.perso.Play("afterJump");
                    break;
                // Joue l'animation de base (immobile)
                default:
                    this.perso.Play("idle");
                    break;
            }
        }

        public void Jump(ref Vector2 move, KeyboardState keyboardState, TiledMapTileLayer mapLayer)
        {
            float direction = 0;

            if (!fly)
            {
                this.perso.Play("beforeJump");
                this.fly = true;
                this.jump = true;
                this.positionSaut = this.position;
            }
            else
            {
                this.perso.Play("jump");
            }

            if (this.positionSaut.Y - this.position.Y < 80 && this.jump && !CheckTop(mapLayer))
            {
                move += new Vector2(0, (float)-this.jumpVelocity);
            }
            else
            {
                this.jump = false;
                this.positionSaut = new Vector2();
            }

            if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left) && !CheckRight(mapLayer))
            {
                move += new Vector2((float)walkVelocity, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right) && !CheckLeft(mapLayer))
            {
                move += new Vector2((float)-walkVelocity, 0);
            }
        }
        public void Gravity(TiledMapTileLayer mapLayer)
        {
            if (!CheckBottom(mapLayer))
            {
                this.Fly = true;
                this.Position += new Vector2(0, (float)this.gravity);
            }
            else
            {
                this.Fly = false;
            }
        }
        public bool CheckBottom(TiledMapTileLayer mapLayer)
        {
            ushort left = (ushort)((this.Position.X - 40 * Niveau1.scale) / mapLayer.TileWidth);
            ushort right = (ushort)((this.Position.X + 40 * Niveau1.scale) / mapLayer.TileWidth);
            ushort y = (ushort)((this.Position.Y + 60 * Niveau1.scale) / mapLayer.TileHeight);

            TiledMapTile? tileLeft;
            TiledMapTile? tileRight;

            if ((mapLayer.TryGetTile(left, y, out tileLeft) != false && !tileLeft.Value.IsBlank) || (mapLayer.TryGetTile(right, y, out tileRight) != false && !tileRight.Value.IsBlank))
                return true;

            return false;
        }
        public bool CheckTop(TiledMapTileLayer mapLayer)
        {
            ushort left = (ushort)((this.Position.X - 50 * Niveau1.scale) / mapLayer.TileWidth);
            ushort right = (ushort)((this.Position.X + 50 * Niveau1.scale) / mapLayer.TileWidth);
            ushort y = (ushort)((this.Position.Y - 60 * Niveau1.scale) / mapLayer.TileHeight);

            TiledMapTile? tileLeft;
            TiledMapTile? tileRight;

            if ((mapLayer.TryGetTile(left, y, out tileLeft) != false && !tileLeft.Value.IsBlank) || (mapLayer.TryGetTile(right, y, out tileRight) != false && !tileRight.Value.IsBlank))
                return true;

            return false;
        }
        public bool CheckLeft(TiledMapTileLayer mapLayer)
        {
            ushort x = (ushort)((this.Position.X - 50 * Niveau1.scale) / mapLayer.TileWidth);
            ushort top = (ushort)((this.Position.Y + 50 * Niveau1.scale) / mapLayer.TileHeight);
            ushort bottom = (ushort)((this.Position.Y - 50 * Niveau1.scale) / mapLayer.TileHeight);

            TiledMapTile? tileTop;
            TiledMapTile? tileBottom;

            if ((mapLayer.TryGetTile(x, top, out tileTop) != false && !tileTop.Value.IsBlank) || (mapLayer.TryGetTile(x, bottom, out tileBottom) != false && !tileBottom.Value.IsBlank))
                return true;

            return false;
        }
        public bool CheckRight(TiledMapTileLayer mapLayer)
        {
            ushort x = (ushort)((this.Position.X + 50 * Niveau1.scale) / mapLayer.TileWidth);
            ushort top = (ushort)((this.Position.Y + 50 * Niveau1.scale) / mapLayer.TileHeight);
            ushort bottom = (ushort)((this.Position.Y - 50 * Niveau1.scale) / mapLayer.TileHeight);

            TiledMapTile? tileTop;
            TiledMapTile? tileBottom;

            if ((mapLayer.TryGetTile(x, top, out tileTop) != false && !tileTop.Value.IsBlank) || (mapLayer.TryGetTile(x, bottom, out tileBottom) != false && !tileBottom.Value.IsBlank))
                return true;

            return false;
        }
    }
}
