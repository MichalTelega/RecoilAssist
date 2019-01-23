using RecoilAssist.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecoilAssist.DatabaseModule
{
    /// <summary>
    /// Klasa pomocnicza służąca do wykonywania pomocniczych operacji dotyczących interfejsu BD.
    /// </summary>
    public static class PatternContextHelper
    {
        /// <summary>
        /// Słownik parujący modele wzorców zapisywanych w BD z instancjami powołanymi w pamięci
        /// </summary>
        private static Dictionary<PatternModel, MovementPattern> ModelObjectDictionary = new Dictionary<PatternModel, MovementPattern>();

        /// <summary>
        /// Zamienia model na instancję wzorca którego można wykorzystać do obliczeń
        /// </summary>
        /// <param name="patternModel">model wzorca</param>
        /// <returns>instancja wzorca</returns>
        public static MovementPattern ToPattern(this PatternModel patternModel)
        {
            // jeżeli w pamięci nie mamy takiej instancji, utwórz nową
            if (!ModelObjectDictionary.ContainsKey(patternModel))
            {
                var pattern = MovementPattern.Create(patternModel.Type,
                    new double[] { patternModel.A, patternModel.B, patternModel.C },
                    patternModel.Direction,
                    patternModel.StartTime,
                    patternModel.StopTime);
                ModelObjectDictionary.Add(patternModel, pattern);
                return pattern;
            }
            // jeżeli już jest taka instancja, po prostu ją zwróć
            else
            {
                return ModelObjectDictionary[patternModel];
            }
        }

        /// <summary>
        /// Zamienia instancję wzorca na model którego można zapisać w BD
        /// </summary>
        /// <param name="pattern">instancja wzorca</param>
        /// <returns>model wzorca</returns>
        public static PatternModel ToModel(this MovementPattern pattern)
        {
            PatternModel patternModel;
            // jeżeli w bazie nie istnieje taki wzorzec, utwórz nowy i naduj mu ID
            if (!ModelObjectDictionary.ContainsValue(pattern))
            {
                patternModel = new PatternModel();
                patternModel.Id = Guid.NewGuid();
                ModelObjectDictionary.Add(patternModel, pattern);
            }
            // w innym wypadku znajdź referencję na istniejący rekord (aby go nadpisać)
            else
            {
                patternModel = ModelObjectDictionary.Keys.First(k => ModelObjectDictionary[k] == pattern);

            }

            patternModel.Type = pattern.Type;
            patternModel.A = pattern.A;
            patternModel.B = pattern.B;
            patternModel.C = pattern.C;
            patternModel.Direction = pattern.Direction;
            patternModel.StartTime = pattern.StartTime;
            patternModel.StopTime = pattern.StopTime;

            return patternModel;
        }
    }
}
