using System;

namespace RecoilAssist.Patterns
{
    // Logarytmiczny wzorzec
    public class LogarithmicPattern : MovementPattern
    {
        public LogarithmicPattern(double[] values, MovementDirection direction, TimeSpan startTime, TimeSpan stopTime) : base(FunctionType.Logarithmic, values, direction, startTime, stopTime) { }

        protected override double Function(double x)
        {
            // A* log(Bx + C)
            return values[0] * Math.Log(values[1] * x + values[2]);
        }

        public override string ToString()
        {
            return $"({values[0]})* log(({values[1]})x + ({values[2]}))";
        }
    }
}
