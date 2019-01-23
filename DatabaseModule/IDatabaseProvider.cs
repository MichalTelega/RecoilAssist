using RecoilAssist.Patterns;
using System;
using System.Collections.Generic;

namespace RecoilAssist.DatabaseModule
{
    /// <summary>
    /// "Dostawca" danych biznesowych, w tym przypadku zapisanych wzorców
    /// </summary>
    public interface IDatabaseProvider : IDisposable
    {
        /// <summary>
        /// Lista wzorców, na której można dodawać/odczytywać/aktualizować/usuwać rekordy (CRUD)
        /// </summary>
        IList<MovementPattern> Patterns { get; }

        /// <summary>
        /// Zapisuje zmiany
        /// </summary>
        void CommitChanges();
    }
}
