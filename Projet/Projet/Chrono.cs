using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet
{
    internal class Chrono
    {
        public static String AffichageChrono(float chrono)
        {
            TimeSpan seconds = TimeSpan.FromSeconds(chrono);
            string formatChrono = seconds.ToString("m':'ss");

            return formatChrono;
        }
    }
}
