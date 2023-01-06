using Microsoft.Xna.Framework;
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
    internal class Recompenses
    {
        private Vector2 position;
        private AnimatedSprite sprite;
        private string typeRecompense;

        public static int effetPoisson =1, effetPiece = -1;

        public Recompenses(Vector2 position, string typeRecompense)
        {
            this.Position = position;
            this.Sprite = sprite;
            this.TypeRecompense = typeRecompense;
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

        public string TypeRecompense
        {
            get
            {
                return this.typeRecompense;
            }

            set
            {
                this.typeRecompense = value;
            }
        }

        public void LoadContent(SpriteSheet sprite)
        {
            Sprite = new AnimatedSprite(sprite);
        }



    }
}
