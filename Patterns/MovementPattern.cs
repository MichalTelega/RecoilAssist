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
        /// Kierunek w jakim b�dzie wyliczony wektor
        /// </summary>
        public MovementDirection Direction { get; set; }

        /// <summary>
        /// Rodzaj funkcji jaki b�dzie wykorzystany do obliczenia wektora
        /// </summary>
        public FunctionType Type { get; private set; }

        /// <summary>
        /// Czas w jakim rozpoczyna si� funkcja. Warto�� wektora poni�ej tego czasu b�dzie wynosi�a (0,0)
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// Czas w jakim funkcja osi�g� maksymaln� warto��. Warto�� wektora powy�ej tego czasu b�dzie wynosi�a tyle w momencie okre�lonym przez t� warto��
        /// </summary>
        public TimeSpan StopTime { get; set; }

        /// <summary>
        /// Warto�ci wykorzystywane do oblicze� tzn. A, B, C
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

            // ustaw bezpiecznie warto�ci
            this.values = new double[3];
            this.values[0] = values.Length > 0 ? values[0] : 0;
            this.values[1] = values.Length > 1 ? values[1] : 0;
            this.values[2] = values.Length > 2 ? values[2] : 0;
        }

        /// <summary>
        /// Funkcja tworz�ca odpowiedni wzorzec. Mo�na ten sam efekt osi�gn�� wywo�uj�c odpowiednie knstruktory,
        /// ale wzorzec "Fabryki" jest w tym przypadku bardzo wygodny
        /// </summary>
        /// <param name="functionType">Typ funkcji</param>
        /// <param name="values">tyblica warto��i funkcji w kolejno�ci: [A, B, C]</param>
        /// <param name="direction">kierunek wektora</param>
        /// <param name="startTime">czas rozpocz�cia</param>
        /// <param name="stopTime">czas zako�czenia</param>
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
        /// Tutaj wykonywane s� obliczenia wzorca wed�ug typu funkcji, wynik ten pos�u�y do utworzenia odpowiedniego wektora
        /// </summary>
        protected abstract double Function(double x);

        /// <summary>
        /// Zwraca wektor wyliczony na podstawie wzorca w odpowiednim fragmencie czasu
        /// </summary>
        /// <param name="time">czas wzgl�dny liczony od rozpocz�cia zdarzenia (klikni�cia myszki)</param>
        public Vector GetDelta(TimeSpan time)
        {
            // ustaw min, max
            if (StartTime > time)
                return Vector.Zero;
            else if (StopTime < time)
                time = StopTime;

            // utaw x na ilo�� sekund liczonych od rozpocz�cia funkcji (nie zdarzenia)
            double x = time.Subtract(StartTime).TotalSeconds;

            // zwr�c wektor
            if (Direction == MovementDirection.Vertical)
                return new Vector(0, (int)Function(x));
            else
                return new Vector((int)Function(x), 0);
        }
    }
}
