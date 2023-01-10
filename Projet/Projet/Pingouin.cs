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
        // Position du pingouin et position de départ lors d'un saut
        private Vector2 position;
        private Vector2 positionSaut;

        private AnimatedSprite perso;
        private RectangleF hitBox;

        private float scale;
        private float rotation;

        // Paramètres de vitesse
        private double walkVelocity;
        private double slideVelocity;
        private double jumpVelocity;
        private double gravityVelocity;

        // Etat du pingouin (en train de glisser, en l'air, en train de sauter, se déplace vers la gauche/droite)
        private bool slideState;
        private bool fly;
        private bool jumpState;
        private bool isMovingLeft;
        private bool isMovingRight;

        private String direction;

        // Life
        private int currentLife;
        private int maxLife;


        // Propriétés
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
        public Vector2 PositionSaut
        {
            get
            {
                return this.positionSaut;
            }

            set
            {
                this.positionSaut = value;
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
        public RectangleF HitBox
        {
            get
            {
                return this.hitBox;
            }

            set
            {
                this.hitBox = value;
            }
        }
        public float Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                this.scale = value;
            }
        }
        public float Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                this.rotation = value;
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
        public double JumpVelocity
        {
            get
            {
                return this.jumpVelocity;
            }

            set
            {
                this.jumpVelocity = value;
            }
        }
        public double GravityVelocity
        {
            get
            {
                return this.gravityVelocity;
            }

            set
            {
                this.gravityVelocity = value;
            }
        }
        public bool SlideState
        {
            get
            {
                return this.slideState;
            }

            set
            {
                this.slideState = value;
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
        public bool JumpState
        {
            get
            {
                return this.jumpState;
            }

            set
            {
                this.jumpState = value;
            }
        }
        public bool IsMovingLeft
        {
            get
            {
                return this.isMovingLeft;
            }

            set
            {
                this.isMovingLeft = value;
            }
        }
        public bool IsMovingRight
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
        public string Direction
        {
            get
            {
                return this.direction;
            }

            set
            {
                this.direction = value;
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


        // Constructeur
        public Pingouin(float x, float y, float scale=1)
        {
            this.Position = new Vector2(x, y);
            this.slideState = false;

            this.gravityVelocity = 2.5;
            this.walkVelocity = 2;
            this.slideVelocity = 2.5;
            this.jumpVelocity = 10;

            this.scale = scale;

            this.MaxLife = 3;
            this.CurrentLife = this.MaxLife;

            this.direction = "Right";
            this.rotation = 0;
        }


        public void Update(float deltaTime, TiledMapTileLayer groundLayer)
        {
            /*
            Fonction d'update du pingouin. Elle permet de centraliser toute les opérations à effectuer lors de chaque frame. 
            */

            // Application de la gravité
            this.Gravity(groundLayer);
            
            this.perso.Update(deltaTime);
        }
        public void Affiche(Game1 game)
        {
            // Affichage du pingouin
            game.SpriteBatch.Draw(this.Perso, this.Position, this.Rotation, new Vector2(scale));
        }
        public void Animate(String animation)
        {
            /* 
            Fonction d'animation du personnage
            */

            // Switch en fonction du nom de l'animation passé en paramètre
            switch (animation)
            {
                // Joue l'animation de célébration
                case "celebrate":
                    this.perso.Play("celebrate");
                    break;
                // Joue l'animation d'attaque
                case "attack":
                    this.perso.Play($"attack{this.direction}");
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
                    if (!this.slideState)
                        this.perso.Play($"beforeSlide{this.direction}");
                    this.perso.Play($"slide{this.direction}");
                    break;
                case "beforeJump":
                    this.perso.Play($"beforeJump{this.direction}");
                    break;
                case "jump":
                    this.perso.Play($"jump{this.direction}");
                    break;
                // Joue l'animation de saut
                case "afterjump":
                    this.perso.Play($"afterJump{this.direction}");
                    break;
                // Joue l'animation de base (immobile)
                default:
                    this.perso.Play("idle");
                    break;
            }
        }

        public void Walk()
        {
            if (this.isMovingRight)
            {
                this.perso.Play("walkForward");
                this.position += new Vector2((float)walkVelocity, 0);
            }
            else
            {
                this.perso.Play("walkBehind");
                this.position += new Vector2((float)-walkVelocity, 0);
            }
        }
        public void Jump(int direction=0)
        {
            /*
            Fonction gérant le saut du pingouin
            */

            // Si le pingouin n'est pas dans les airs,
            // il joue une animation et on attribue ces valeurs aux variables suivantes
            if (!fly)
            {
                this.Animate("beforejump");

                this.fly = true;
                this.jumpState = true;
                this.positionSaut = this.position;
            }
            // S'il est déjà dans les airs, il joue l'animation suivante
            else
            {
                this.Animate("jump");
            }

            // Si le pingouin est en train de sauter,
            // que la différence de hauteur entre sa position au moment du saut et sa position actuelle est inférieur à 80
            // et qu'il n'y a pas d'obstacles au-dessus de lui, on applique un mouvement vertical
            // (Cela permet de fluidifier le mouvement de saut et de ne pas téléporter le pingouin)
            if (this.jumpState && this.positionSaut.Y - this.position.Y < 100)
            {
                this.position += new Vector2(0, (float)-this.jumpVelocity);
            }
            // Sinon il n'est plus en train de sauter, il est en phase descendante
            else
            {
                this.jumpState = false;
                this.positionSaut = new Vector2();
            }

            // Si l'utilisateur presse la flèche de droite ou de gauche, la direction est appliquée, sinon multiplication par 0
            this.position += new Vector2((float)walkVelocity, 0) * direction;
        }
        public void Slide(int direction=0)
        {
            this.Animate("slide");
            // S'il n'est pas encore en train de glisser, joue l'animation où il se jète
            if (!this.slideState)
            {
                this.slideState = true;
            }

            this.position += new Vector2((float)slideVelocity, 0) * direction;
        }
        public void Gravity(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction appliquant la gravité au pingouin
            */

            // S'il n'y a pas d'obstacle en-dessous du pingouin, le pingouin est en l'air
            // on applique un mouvement vertical vers le bas
            if (!Collision.MapCollision(this.CheckBottom(), mapLayer))
            {
                System.Diagnostics.Debug.WriteLine(!Collision.MapCollision(this.CheckBottom(), mapLayer));
                this.fly = true;
                this.position += new Vector2(0, (float)this.gravityVelocity);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Not flying");
                this.fly = false;
            }
        }

        public Point[] CheckBottom()
        {
            /*
            Fonction vérifiant la présence d'obsctale en-dessous du pingouin
            */

            // Définition de deux points et de deux tiles, en bas à gauche et en bas à droite
            int left = (int)(this.Position.X - 40 * this.scale);
            int middle = (int)this.Position.X;
            int right = (int)(this.Position.X + 40 * this.scale);
            int y = (int)(this.Position.Y + 60 * this.scale);

            return new Point[] { new Point(left, y), new Point(middle, y), new Point(right, y) };
        }
        public Point[] CheckTop()
        {
            /*
            Fonction vérifiant la présence d'obsctale au-dessus du pingouin
            */

            // Définition de deux points et de deux tiles, en haut à gauche et en haut à droite
            int left = (int)(this.Position.X - 40 * this.scale);
            int middle = (int)this.Position.X;
            int right = (int)(this.Position.X + 40 * this.scale);
            int y = (int)(this.Position.Y - 40 * this.scale);

            return new Point[] { new Point(left, y), new Point(middle, y), new Point(right, y) };
        }
        public Point[] CheckLeft()
        {
            /*
            Fonction vérifiant la présence d'obsctale à gauche du pingouin
            */

            // Définition de deux points et de deux tiles, en haut à gauche et en bas à gauche
            int x = (int)(this.Position.X - 50 * this.scale);
            int top = (int)(this.Position.Y + 50 * this.scale);
            int middle = (int)(this.Position.Y + 10 * this.scale);
            int bottom = (int)(this.Position.Y - 30 * this.scale);

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            return new Point[] { new Point(x, top), new Point(x, middle), new Point(x, bottom) };
        }
        public Point[] CheckRight()
        {
            /*
            Fonction vérifiant la présence d'obsctale à droite du pingouin
            */

            // Définition de deux points et de deux tiles, en haut à droite et en bas à droite
            int x = (int)(this.Position.X + 50 * this.scale);
            int top = (int)(this.Position.Y + 50 * this.scale);
            int middle = (int)(this.Position.Y + 10 * this.scale);
            int bottom = (int)(this.Position.Y - 30 * this.scale);

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            return new Point[] { new Point(x, top), new Point(x, middle), new Point(x, bottom) };
        }

        public void TakeDamage(int damage, ref double invincibilityChrono)
        {

            if (invincibilityChrono > 2)
            {
                CurrentLife -= damage;
                invincibilityChrono = 0;
            }
        }
        public void Heal(int healPoints)
        {
            CurrentLife += healPoints;
        }
    }
}