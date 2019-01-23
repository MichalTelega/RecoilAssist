using System;

namespace RecoilAssist.Patterns
{
    public class RationalPattern : MovementPattern
    {
        public RationalPattern(double[] values, MovementDirection direction, TimeSpan startTime, TimeSpan stopTime) : base(FunctionType.Rational, values, direction, startTime, stopTime) { }

        protected override double Function(double x)
        {
            // A / (Bx + C)
            return values[0] * Math.Log(values[1] * x + values[2]);
        }

        public override string ToString()
        {
            return $"({values[0]}) / (({values[1]})x + ({values[2]}))";
        }
    }
}
