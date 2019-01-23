using RecoilAssist.Patterns;
using System;

namespace RecoilAssist.DatabaseModule
{
    /// <summary>
    /// Model "bazodanowy" wzorca. Przechowuje wartości definiujące wzorzec.
    /// Rozróżnienie wzorca na model który używamy do przechowywania informacji w BD
    /// i na faktyczną instancję w programie na celu parę efektów:
    ///     - oddzielenie implementacji programu od komunikacji z BD
    ///     - przyspieszenie obliczeń wzorca o parę instrukcji (niewiele, ale w tym przypadku bardzo cenne)
    ///     - potrzeba użycia wartości ID w BD, a w przypadku programu nie koniecznie
    ///     - możliwość odfiltrowania niepotrzebnych informacji
    /// </summary>
    public class PatternModel
    {
        public Guid Id { get; set; }

        public MovementDirection Direction { get; set; }

        public FunctionType Type { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan StopTime { get; set; }

        public double A { get; set; }

        public double B { get; set; }

        public double C { get; set; }
    }
}
