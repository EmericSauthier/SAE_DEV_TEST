using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Screens;

namespace Projet
{
    internal class Regle : GameScreen
    {
        private Game1 _myGame;

        public static SpriteFont policePetite;

        //SPRITE DECO
        private Pingouin _pingouinSauter;
        private Pingouin _pingouinGlisser;
        private Pingouin _pingouinAvancer;
        private Recompenses _portail;
        private Recompenses _piece;

        //SOURIS POUR GERER CLIC
        private MouseState _mouseState;

        //CHAMPS TEXTE
        private string _menuTXT;
        private string _texteEnnemi;
        private string _texteControle;
        private string _texteIntroEnnemi;
        private string _texteMonaie;
        private string _texteMonaieCas1;
        private string _texteMonaieCas2;
        private string _texteTeleporteur;
        private string _texteTeleporteurDesc;
        private Vector2 _positionMenu;
        private Vector2 _positiontxtEnnemi;
        private Vector2 _positiontxtControle;
        private Vector2 _positiontxtIntroEnnemi;
        private Vector2 _positiontxtMonaie;
        private Vector2 _positionMonaieCas1;
        private Vector2 _positionMonaieCas2;
        private Vector2 _positiontxtTeleport;
        private Vector2 _positiontxtTeleportDesc;
            
            //EXPLI DEPLACEMENT
        private string _sauter;
        private string _glisser;
        private string _avancer;
        private string _attaquer;
        private string _relever;
        private Vector2 _positionSauter;
        private Vector2 _positionGlisser;
        private Vector2 _positionAvancer;
        private Vector2 _positionAttaquer;
        private Vector2 _positionRelever;

        public Regle(Game1 game) : base(game)
        {
            _myGame = game;
        }
        public override void Initialize()
        {
            //INITIALISATION DES TEXTES
            _menuTXT = "Menu";
                //DEPLACEMENT
            _texteControle = "Action :";
            _avancer = "Avencer : fleche droite/gauche";
            _sauter = "Sauter : espace";
            _glisser = "Glisser : fleche du bas + direction";
            _attaquer = "Boule de neige : entrer";
                //ENNEMI
            _texteEnnemi = "Attention aux predateurs !";
            _texteIntroEnnemi = "Durant votre parcours, faites attention aux ennemis sur votre chemin, ils pourraient bien vous devorer ! \nEviter les ou attaquer avec la boule de neige ou en sautant sur leur dos !";
                //Recoltable
            _texteMonaie = "Monnaie viscieuse...";
            _texteMonaieCas1 = "Lorsque la vie n'est pas maximal : ajoute un coeur";
            _texteMonaieCas2 = "Lorsqque vie pleine 50% de risque d'etre ralentit et 50% de chance d'etre accelere";
            _texteTeleporteur = "Morceau de portail";
            _texteTeleporteurDesc = " Recolter les tous afin de faire apparaitre le teleporteur sur la fin de la map !";

            //POSITION
            _positionMenu = new Vector2(30, 50);

            _positiontxtControle = new Vector2(50, 150);
            _positionAvancer = new Vector2(_positionMenu.X, _positiontxtControle.Y + 50);
            _positionSauter = new Vector2(_positionMenu.X+100, _positionAvancer.Y + 70);
            _positionGlisser = new Vector2(_positionMenu.X, _positionSauter.Y + 60);
            _positionRelever = new Vector2(_positionMenu.X, _positionGlisser.Y + 30);
            _positionAttaquer = new Vector2(_positionMenu.X, _positionRelever.Y + 20);

            _positiontxtEnnemi = new Vector2(_positiontxtControle.X, _positionAttaquer.Y+60);
            _positiontxtIntroEnnemi = new Vector2(_positionMenu.X, _positiontxtEnnemi.Y+35);

            _positiontxtMonaie = new Vector2(_positiontxtEnnemi.X, _positiontxtIntroEnnemi.Y + 60);
            _positionMonaieCas1= new Vector2(_positionMenu.X, _positiontxtMonaie.Y + 35);
            _positionMonaieCas2 = new Vector2(_positionMenu.X, _positionMonaieCas1.Y + 15);

            _positiontxtTeleport = new Vector2(_positiontxtEnnemi.X, _positionMonaieCas2.Y + 60);
            _positiontxtTeleportDesc = new Vector2(_positionMenu.X, _positiontxtTeleport.Y + 35);


            //CREATION DECO
            _pingouinSauter = new Pingouin(_positionSauter.X-80, _positionSauter.Y);
            _pingouinAvancer = new Pingouin(_positionAvancer.X+_avancer.Length*12, _positionAvancer.Y-20);
            _pingouinGlisser = new Pingouin(_positionGlisser.X+_glisser.Length*10, _positionGlisser.Y);
            _portail = new Recompenses(new Vector2(_positiontxtTeleportDesc.X+600, _positiontxtTeleportDesc.Y), "portal", 0);
            _piece = new Recompenses(new Vector2(_positiontxtMonaie.X+400, _positionMonaieCas1.Y-15), "coin", 0);

            base.Initialize();
        }
        public override void LoadContent()
        {
            //PINGOUIN DECO
            SpriteSheet spriteSheet = Content.Load<SpriteSheet>("Perso/penguin.sf", new JsonContentLoader());
            SpriteSheet spriteSheetPortal = Content.Load<SpriteSheet>("Decors/portal.sf", new JsonContentLoader());
            SpriteSheet spriteSheetCoin = Content.Load<SpriteSheet>("Decors/spritCoin.sf", new JsonContentLoader());
            _pingouinSauter.Perso = new AnimatedSprite(spriteSheet);
            _pingouinGlisser.Perso = new AnimatedSprite(spriteSheet);
            _pingouinAvancer.Perso = new AnimatedSprite(spriteSheet);
            _portail.LoadContent(spriteSheetPortal);
            _piece.LoadContent(spriteSheetCoin);
            

            //POLICE
            policePetite = Content.Load<SpriteFont>("Font/FontRulesDesc");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            //ANIMATION DU PINGOUIN DECO
            float deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _pingouinSauter.Animate("jump");
            _pingouinAvancer.Animate("walkForward");
            _pingouinGlisser.Animate("slide");
            _pingouinSauter.Perso.Update(deltaSeconds);
            _pingouinAvancer.Perso.Update(deltaSeconds);
            _pingouinGlisser.Perso.Update(deltaSeconds);
            _portail.Sprite.Play("portal");
            _portail.Sprite.Update(deltaSeconds);
            _piece.Sprite.Play("coin");
            _piece.Sprite.Update(deltaSeconds);

            //GESTION CLIC
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                //si clic retour au menu
                if (_mouseState.X >= _positionMenu.X && _mouseState.Y >= _positionMenu.Y && _mouseState.X <= _positionMenu.X + _menuTXT.Length * 24 && _mouseState.Y <= _positionMenu.Y + 24)
                {
                    _myGame.clicMenu = true;
                }


            }
        }

        public override void Draw(GameTime gameTime)
        {
            _myGame.GraphicsDevice.Clear(Color.Navy);
            _myGame.SpriteBatch.Begin();
            //_myGame.SpriteBatch.Draw(_textureFond, _positionFond, Color.White);//LE FOND

            _myGame.SpriteBatch.DrawString(Game1.police, $"{_menuTXT}", _positionMenu, Color.White);

            _myGame.SpriteBatch.DrawString(Game1.police, $"{_texteControle}", _positiontxtControle, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_sauter}", _positionSauter, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_glisser}", _positionGlisser, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_avancer}", _positionAvancer, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_relever}", _positionRelever, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_attaquer}", _positionAttaquer, Color.White);
            //PINGOUIN
            _myGame.SpriteBatch.Draw(_pingouinSauter.Perso, _pingouinSauter.Position);
            _myGame.SpriteBatch.Draw(_pingouinAvancer.Perso, _pingouinAvancer.Position);
            _myGame.SpriteBatch.Draw(_pingouinGlisser.Perso, _pingouinGlisser.Position);
            _myGame.SpriteBatch.Draw(_portail.Sprite, _portail.Position, 0, new Vector2(2));
            _myGame.SpriteBatch.Draw(_piece.Sprite, _piece.Position, 0, new Vector2((float)0.2));

            _myGame.SpriteBatch.DrawString(Game1.police, $"{_texteEnnemi}", _positiontxtEnnemi, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_texteIntroEnnemi}", _positiontxtIntroEnnemi, Color.White);


            _myGame.SpriteBatch.DrawString(Game1.police, $"{_texteMonaie}", _positiontxtMonaie, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_texteMonaieCas1}", _positionMonaieCas1, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_texteMonaieCas2}", _positionMonaieCas2, Color.White);

            _myGame.SpriteBatch.DrawString(Game1.police, $"{_texteTeleporteur}", _positiontxtTeleport, Color.White);
            _myGame.SpriteBatch.DrawString(policePetite, $"{_texteTeleporteurDesc}", _positiontxtTeleportDesc, Color.White);


            _myGame.SpriteBatch.End();
        }
    }
}
