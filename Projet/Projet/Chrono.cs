using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet
{
    internal class Chrono
    {
        public static double chrono;
        public static double chronoTrap;
        public static double chronoDepEagle;
        public static double chronoInvincibility;

        public static String AffichageChrono(double chrono)
        {
            TimeSpan seconds = TimeSpan.FromSeconds(chrono);
            string formatChrono = seconds.ToString("m':'ss");

            return formatChrono;
        }

        public static void InitializeChronos()
        {
            chrono = 0;
            chronoInvincibility = 0;
        }

        public static void UpdateChronos(float deltaSeconds)
        {
            Chrono.chrono += deltaSeconds;
            Chrono.chronoInvincibility += deltaSeconds;
        }
    }
}
