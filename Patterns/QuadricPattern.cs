using System;

namespace RecoilAssist.Patterns
{
    // Kwadratowy wzorzec
    public class QuadricPattern : MovementPattern
    {
        public QuadricPattern(double[] values, MovementDirection direction, TimeSpan startTime, TimeSpan stopTime) : base(FunctionType.Quadratic, values, direction, startTime, stopTime) { }

        protected override double Function(double x)
        {
            // ax2 + bx + c
            return values[2] * Math.Pow(x, 2) + values[1] * x + values[0];
        }

        public override string ToString()
        {
            return $"({values[0]})x^2 + ({values[1]})x + ({values[2]})";
        }
    }
}
