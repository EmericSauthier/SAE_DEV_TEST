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

        // Paramètres de vitesse
        private double walkVelocity;
        private double slideVelocity;
        private double jumpVelocity;
        private double gravity;

        // Etat du pingouin (en train de glisser, en l'air, en train de sauter)
        private bool slide;
        private bool fly;
        private bool jump;
        public bool isMovingRight;

        // Life
        private int currentLife;
        private int maxLife;

        // Tableau de boule de neige
        private Snowball[] snowballs;

        // Constructeur
        public Pingouin(float x, float y, float scale=1)
        {
            this.Position = new Vector2(x, y);
            this.slide = false;

            this.gravity = 2.5;
            this.walkVelocity = 2;
            this.slideVelocity = 2.5;
            this.jumpVelocity = 10;

            this.scale = scale;

            this.snowballs = new Snowball[0];

            this.MaxLife = 3;
            this.CurrentLife = this.MaxLife;
        }
        
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
        internal Snowball[] Snowballs
        {
            get
            {
                return this.snowballs;
            }

            set
            {
                this.snowballs = value;
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

        public void Update(bool gameOver, float deltaTime, KeyboardState keyboardState, TiledMapTileLayer mapLayer)
        {
            /*
            Fonction d'update du pingouin. Elle permet de centraliser toute les opérations à effectuer lors de chaque frame. 
            */

            // Application de la gravité
            this.Gravity(mapLayer);

            // Application d'un mouvement si commander par l'utilisateur
            this.InputsManager(gameOver, keyboardState, mapLayer);
            this.perso.Update(deltaTime);

            this.SnowballsUpdate(mapLayer);
        }

        public void InputsManager(bool gameOver, KeyboardState keyboardState, TiledMapTileLayer mapLayer)
        {
            /*
            Fonction permettant de lancer les animations et de faire bouger le pingouin si les entrées correspondent
            */

            Vector2 move = Vector2.Zero;

            // Vérification de l'état de la flèche du bas
            if (keyboardState.IsKeyUp(Keys.Down))
            {
                this.slide = false;
            }
            // Vérification de l'état de la touche entrée
            if (keyboardState.IsKeyUp(Keys.Enter))
            {
                this.Attack();
            }

            // Si le jeu est fini
            if (gameOver)
            {
                this.perso.Play("celebrate");
            }
            // Si le pingouin saute (touche espace) ou est dans les airs
            else if ((keyboardState.IsKeyDown(Keys.Space) || this.fly))
            {
                Jump(ref move, keyboardState, mapLayer);
            }
            // Si le pingouin va à droite (flèche droite uniquement)
            else if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left))
            {
                isMovingRight = true;
                this.perso.Play("walkForward");
                // Vérifie qu'il n'y a pas d'obstacles à droite
                if (!CheckRight(mapLayer))
                    move = new Vector2((float)walkVelocity, 0);
            }
            // Si le pingouin va à gauche (flèche gauche uniquement)
            else if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right))
            {
                isMovingRight = false;
                this.perso.Play("walkBehind");
                // Vérifie qu'il n'y a pas d'obstacles à gauche
                if (!CheckLeft(mapLayer))
                    move = new Vector2((float)-walkVelocity, 0);
            }
            // Si le pingouin glisse (flèche du bas)
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                isMovingRight = true;

                // S'il n'est pas encore en train de glisser, joue l'animation où il se jète
                if (!this.slide)
                {
                    this.perso.Play("beforeSlide");
                    this.slide = true;
                }
                // S'il est déjà en trai de glisser, joue l'animation à plat ventre
                else
                {
                    this.perso.Play("slide");
                }
                // Vérifie qu'il n'y a pas d'obstacles à gauche
                if (!CheckRight(mapLayer))
                    move = new Vector2((float)slideVelocity, 0);
            }
            // Si aucun mouvement n'est demandé, il reste immobile
            else
            {
                this.perso.Play("idle");
            }

            // Applique le mouvement à la position du pingouin
            this.position += move;
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
                    this.perso.Play("Attack");
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

        public void Attack()
        {
            // Joue l'animation d'attaque
            this.Animate("Attack");

            // Ajoute une boule de neige au tableau
            Snowball[] newSnowballsArray = new Snowball[this.snowballs.Length + 1];
            for (int i = 0; i < this.snowballs.Length; i++)
            {
                newSnowballsArray[i] = this.snowballs[i];
            }
            newSnowballsArray[newSnowballsArray.Length - 1] = new Snowball(this.Position.X, this.Position.Y, this.snowballTexture);
            this.snowballs = newSnowballsArray;
        }
        public void Jump(ref Vector2 move, KeyboardState keyboardState, TiledMapTileLayer mapLayer)
        {
            /*
            Fonction gérant le saut du pingouin
            */

            // Si le pingouin n'est pas dans les airs,
            // il joue une animation et on attribue ces valeurs aux variables suivantes
            if (!fly)
            {
                this.perso.Play("beforeJump");
                this.fly = true;
                this.jump = true;
                this.positionSaut = this.position;
            }
            // S'il est déjà dans les airs, il joue l'animation suivante
            else
            {
                this.perso.Play("jump");
            }

            // Si le pingouin est en train de sauter,
            // que la différence de hauteur entre sa position au moment du saut et sa position actuelle est inférieur à 80
            // et qu'il n'y a pas d'obstacles au-dessus de lui, on applique un mouvement vertical
            // (Cela permet de fluidifier le mouvement de saut et de ne pas téléporter le pingouin)
            if (this.jump && this.positionSaut.Y - this.position.Y < 100 && !CheckTop(mapLayer))
            {
                move += new Vector2(0, (float)-this.jumpVelocity);
            }
            // Sinon il n'est plus en train de sauter, il est en phase descendante
            else
            {
                this.jump = false;
                this.positionSaut = new Vector2();
            }

            // Si l'utilisateur presse la flèche de droite et qu'il n'y a pas d'obstacles à droite,
            // on applique un mouvement horizontal vers la droite
            if (keyboardState.IsKeyDown(Keys.Right) && !keyboardState.IsKeyDown(Keys.Left) && !CheckRight(mapLayer))
            {
                move += new Vector2((float)walkVelocity, 0);
            }
            // Si l'utilisateur presse la flèche de gauche et qu'il n'y a pas d'obstacles à gauche,
            // on applique un mouvement horizontal vers la gauche
            else if (keyboardState.IsKeyDown(Keys.Left) && !keyboardState.IsKeyDown(Keys.Right) && !CheckLeft(mapLayer))
            {
                move += new Vector2((float)-walkVelocity, 0);
            }
        }
        public void Gravity(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction appliquant la gravité au pingouin
            */

            // S'il n'y a pas d'obstacle en-dessous du pingouin, le pingouin est en l'air
            // on applique un mouvement vertical vers le bas
            if (!CheckBottom(mapLayer))
            {
                this.fly = true;
                this.position += new Vector2(0, (float)this.gravity);
            }
            else
            {
                this.fly = false;
            }
        }

        public bool CheckBottom(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction vérifiant la présence d'obsctale en-dessous du pingouin
            */

            // Définition de deux points et de deux tiles, en bas à gauche et en bas à droite
            ushort left = (ushort)((this.Position.X - 40 * this.scale) / mapLayer.TileWidth);
            ushort right = (ushort)((this.Position.X + 40 * this.scale) / mapLayer.TileWidth);
            ushort y = (ushort)((this.Position.Y + 60 * this.scale) / mapLayer.TileHeight);

            TiledMapTile? tileLeft;
            TiledMapTile? tileRight;

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            if ((mapLayer.TryGetTile(left, y, out tileLeft) != false && !tileLeft.Value.IsBlank) || (mapLayer.TryGetTile(right, y, out tileRight) != false && !tileRight.Value.IsBlank))
                return true;

            return false;
        }
        public bool CheckTop(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction vérifiant la présence d'obsctale au-dessus du pingouin
            */

            // Définition de deux points et de deux tiles, en haut à gauche et en haut à droite
            ushort left = (ushort)((this.Position.X - 40 * this.scale) / mapLayer.TileWidth);
            ushort right = (ushort)((this.Position.X + 40 * this.scale) / mapLayer.TileWidth);
            ushort y = (ushort)((this.Position.Y - 40 * this.scale) / mapLayer.TileHeight);

            TiledMapTile? tileLeft;
            TiledMapTile? tileRight;

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            if ((mapLayer.TryGetTile(left, y, out tileLeft) != false && !tileLeft.Value.IsBlank) || (mapLayer.TryGetTile(right, y, out tileRight) != false && !tileRight.Value.IsBlank))
                return true;

            return false;
        }
        public bool CheckLeft(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction vérifiant la présence d'obsctale à gauche du pingouin
            */

            // Définition de deux points et de deux tiles, en haut à gauche et en bas à gauche
            ushort x = (ushort)((this.Position.X - 50 * this.scale) / mapLayer.TileWidth);
            ushort top = (ushort)((this.Position.Y + 50 * this.scale) / mapLayer.TileHeight);
            ushort bottom = (ushort)((this.Position.Y - 30 * this.scale) / mapLayer.TileHeight);

            TiledMapTile? tileTop;
            TiledMapTile? tileBottom;

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            if ((mapLayer.TryGetTile(x, top, out tileTop) != false && !tileTop.Value.IsBlank) || (mapLayer.TryGetTile(x, bottom, out tileBottom) != false && !tileBottom.Value.IsBlank))
                return true;

            return false;
        }
        public bool CheckRight(TiledMapTileLayer mapLayer)
        {
            /*
            Fonction vérifiant la présence d'obsctale à droite du pingouin
            */

            // Définition de deux points et de deux tiles, en haut à droite et en bas à droite
            ushort x = (ushort)((this.Position.X + 50 * this.scale) / mapLayer.TileWidth);
            ushort top = (ushort)((this.Position.Y + 50 * this.scale) / mapLayer.TileHeight);
            ushort bottom = (ushort)((this.Position.Y - 30 * this.scale) / mapLayer.TileHeight);

            TiledMapTile? tileTop;
            TiledMapTile? tileBottom;

            // Récupération des différentes tiles, si l'une a une valeur, il y a collision
            if ((mapLayer.TryGetTile(x, top, out tileTop) != false && !tileTop.Value.IsBlank) || (mapLayer.TryGetTile(x, bottom, out tileBottom) != false && !tileBottom.Value.IsBlank))
                return true;

            return false;
        }

        public void SnowballsUpdate(TiledMapTileLayer mapLayer)
        {
            int countNull = 0;
            for (int i = 0; i < this.snowballs.Length; i++)
            {
                if (this.snowballs[i].Collide(mapLayer)) // Vérification des collisions avec le décor
                {
                    this.snowballs[i] = null;
                    countNull++;
                }
                else
                {
                    this.snowballs[i].Move();
                }
            }

            Snowball[] temp = this.snowballs;
            this.snowballs = new Snowball[this.snowballs.Length - countNull];
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != null)
                {
                    this.snowballs[i] = temp[i];
                }
            }
        }

        public void TakeDamage(int damage, ref float invincibilityChrono)
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