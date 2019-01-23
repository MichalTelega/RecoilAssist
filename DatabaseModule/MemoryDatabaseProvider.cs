using RecoilAssist.Patterns;
using System.Collections.Generic;

namespace RecoilAssist.DatabaseModule
{
    /// <summary>
    /// DatabaseProvider który nie korzysta z żadnej BD, tylko używa pamięci programu.
    /// Po zakończeniu działania aplikacji dane zostają utracone.
    /// </summary>
    public class MemoryDatabaseProvider : IDatabaseProvider
    {
        /// <summary>
        /// <see cref="IDatabaseProvider.Patterns"/>
        /// </summary>
        public IList<MovementPattern> Patterns { get; private set; }

        public MemoryDatabaseProvider()
        {
            Patterns = new List<MovementPattern>();
        }

        /// <summary>
        /// <see cref="IDatabaseProvider.CommitChanges"/>
        /// </summary>
        public void CommitChanges() { }

        public void Dispose() { }
    }
}
