using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet
{
    internal class Champ
    {
        //CHAMPS POUR MENU
        public static string regle;
        public static Vector2 positionRegle;
        public static string jouer;
        public static Vector2 positionJouer;
        public static string niv;
        public static Vector2 positionNiv;
        public static SpriteFont police;
        public static string quitter;
        public static Vector2 positionQuitter;

        //CHAMPS POUR WIN
        public static string messageGagner;
        public static Vector2 positionMessageGagner;
        public static string messageNivSuiv;
        public static Vector2 positionMessageNivSuiv;
        public static string messageMenu;
        public static Vector2 positionMessageMenu;

        //CHAMPS POUR GAMEOVER
        public static string messagePerdu;
        public static Vector2 positionMessagePerdu;
        public static string messageRejouer;
        public static Vector2 positionMessageRejouer;

        public static void Initialize()
        {
            //CHAMPS POUR MENU
            regle = "Notes de pingouin";
            jouer = "Jouer";
            niv = "Charger un niveau";
            quitter = "Quitter";
            float tailleRegle = 24 * regle.Length;
            positionRegle = new Vector2(tailleRegle / 2, 50);
            positionJouer = new Vector2(tailleRegle / 2, 150);
            positionNiv = new Vector2(tailleRegle / 2, 250);
            positionNiv = new Vector2(tailleRegle / 2, 250);
            positionQuitter = new Vector2(tailleRegle / 2, 350);

            //CHAMPS POUR WIN
            messageGagner = "Niveau completer !";
            messageMenu = "Menu";
            messageNivSuiv = "Niveau suivant -->";
            positionMessageGagner = new Vector2(50, 50);
            positionMessageMenu = new Vector2(50, 350);
            positionMessageNivSuiv = new Vector2(250, 350);

            //CHAMPS POUR GAMEOVER
            messagePerdu = "C'est mort...";
            messageRejouer = "Reessayer";
            positionMessagePerdu = new Vector2(50, 50);
            positionMessageRejouer = new Vector2(50, 350);
        }
    }
}
