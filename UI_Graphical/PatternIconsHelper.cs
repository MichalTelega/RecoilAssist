using RecoilAssist.Patterns;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecoilAssist.UI_Graphical
{
    /// <summary>
    /// Klasa pomocniczna która zwraca odpowiedni obrazek dla podanego typu funkcji
    /// </summary>
    public static class PatternIconsHelper
    {
        /// <summary>
        /// Zwraca obrazek reprezentujący postać funkcji
        /// </summary>
        public static Image Icon(this FunctionType type)
        {
            switch (type)
            {
                case FunctionType.Exponential:
                    return Properties.Resources.Exponential;
                case FunctionType.Quadratic:
                    return Properties.Resources.Quadratic;
                case FunctionType.Logarithmic:
                    return Properties.Resources.Logarithmic;
                case FunctionType.Rational:
                    return Properties.Resources.Rational;
                default:
                    return null;
            }
        }
    }
}
