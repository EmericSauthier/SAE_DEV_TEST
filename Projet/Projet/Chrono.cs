using System;

namespace Projet
{
    internal class Chrono
    {
        public static double chrono;
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
