using System;

namespace RecoilAssist.Patterns
{
    // Wykładniczy wzorzec
    public class ExponentialPattern : MovementPattern
    {
        public ExponentialPattern(double[] values, MovementDirection direction, TimeSpan startTime, TimeSpan stopTime) : base(FunctionType.Exponential, values, direction, startTime, stopTime) { }

        protected override double Function(double x)
        {
            // (A)^(Bx + C)
            return Math.Pow(values[0], values[1] * x + values[2]);
        }

        public override string ToString()
        {
            return $"({values[0]})^(({values[1]})x + ({values[2]}))";
        }
    }
}
