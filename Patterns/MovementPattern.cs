using RecoilAssist.Controllers;
using System;

namespace RecoilAssist.Patterns
{
    /// <summary>
    /// Wzorzec ruchu myszki. Wzorzec ten odpowiada za obliczenie wektora dla odpowiedniego fragmentu czasu.
    /// </summary>
    public abstract class MovementPattern
    {
        /// <summary>
        /// Kierunek w jakim bêdzie wyliczony wektor
        /// </summary>
        public MovementDirection Direction { get; set; }

        /// <summary>
        /// Rodzaj funkcji jaki bêdzie wykorzystany do obliczenia wektora
        /// </summary>
        public FunctionType Type { get; private set; }

        /// <summary>
        /// Czas w jakim rozpoczyna siê funkcja. Wartoœæ wektora poni¿ej tego czasu bêdzie wynosi³a (0,0)
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Czas w jakim funkcja osi¹g¹ maksymaln¹ wartoœæ. Wartoœæ wektora powy¿ej tego czasu bêdzie wynosi³a tyle w momencie okreœlonym przez t¹ wartoœæ
        /// </summary>
        public TimeSpan StopTime { get; set; }

        /// <summary>
        /// Wartoœci wykorzystywane do obliczeñ tzn. A, B, C
        /// </summary>
        protected double[] values;

        public double A { get => values[0]; set => values[0] = value; }
        public double B { get => values[1]; set => values[1] = value; }
        public double C { get => values[2]; set => values[2] = value; }

        public MovementPattern(FunctionType functionType, double[] values, MovementDirection direction, TimeSpan startTime, TimeSpan stopTime)
        {
            Type = functionType;
            Direction = direction;
            StartTime = startTime;
            StopTime = stopTime;

            // ustaw bezpiecznie wartoœci
            this.values = new double[3];
            this.values[0] = values.Length > 0 ? values[0] : 0;
            this.values[1] = values.Length > 1 ? values[1] : 0;
            this.values[2] = values.Length > 2 ? values[2] : 0;
        }

        /// <summary>
        /// Funkcja tworz¹ca odpowiedni wzorzec. Mo¿na ten sam efekt osi¹gn¹æ wywo³uj¹c odpowiednie knstruktory,
        /// ale wzorzec "Fabryki" jest w tym przypadku bardzo wygodny
        /// </summary>
        /// <param name="functionType">Typ funkcji</param>
        /// <param name="values">tyblica wartoœæi funkcji w kolejnoœci: [A, B, C]</param>
        /// <param name="direction">kierunek wektora</param>
        /// <param name="startTime">czas rozpoczêcia</param>
        /// <param name="stopTime">czas zakoñczenia</param>
        /// <returns></returns>
        public static MovementPattern Create(FunctionType functionType, double[] values, MovementDirection direction, TimeSpan startTime, TimeSpan stopTime)
        {
            switch (functionType)
            {
                case FunctionType.Quadratic:
                    return new QuadricPattern(values, direction, startTime, stopTime);
                case FunctionType.Rational:
                    return new RationalPattern(values, direction, startTime, stopTime);
                case FunctionType.Logarithmic:
                    return new LogarithmicPattern(values, direction, startTime, stopTime);
                case FunctionType.Exponential:
                    return new ExponentialPattern(values, direction, startTime, stopTime);
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Tutaj wykonywane s¹ obliczenia wzorca wed³ug typu funkcji, wynik ten pos³u¿y do utworzenia odpowiedniego wektora
        /// </summary>
        protected abstract double Function(double x);

        /// <summary>
        /// Zwraca wektor wyliczony na podstawie wzorca w odpowiednim fragmencie czasu
        /// </summary>
        /// <param name="time">czas wzglêdny liczony od rozpoczêcia zdarzenia (klikniêcia myszki)</param>
        public Vector GetDelta(TimeSpan time)
        {
            // ustaw min, max
            if (StartTime > time)
                return Vector.Zero;
            else if (StopTime < time)
                time = StopTime;

            // utaw x na iloœæ sekund liczonych od rozpoczêcia funkcji (nie zdarzenia)
            double x = time.Subtract(StartTime).TotalSeconds;

            // zwróc wektor
            if (Direction == MovementDirection.Vertical)
                return new Vector(0, (int)Function(x));
            else
                return new Vector((int)Function(x), 0);
        }
    }
}
