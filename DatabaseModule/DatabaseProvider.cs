using RecoilAssist.Patterns;
using System.Collections.Generic;
using System.ComponentModel;

namespace RecoilAssist.DatabaseModule
{
    /// <summary>
    /// DatabaseProvider korzystający z EntityFramework dla platformy .NET w wersji 6.x
    /// Wszelkie operację na bazie danych są schowane pod implementacją tej klasy
    /// </summary>
    public class DatabaseProvider : IDatabaseProvider
    {
        /// <summary>
        /// BindingList który "nasłuchuje" wszelkich zmian dokonywanych na liście
        /// </summary>
        private BindingList<MovementPattern> patternsBindingList = new BindingList<MovementPattern>();

        /// <summary>
        /// "Migawka" z bazy danych, dzięki niej jesteśmy w stanie określić co należy dodać a co usunąć
        /// </summary>
        private List<MovementPattern> patternsInDatabase = new List<MovementPattern>();

        /// <summary>
        /// Wzorce czekające na dodanie do BD
        /// </summary>
        private List<MovementPattern> patternsToAdd = new List<MovementPattern>();

        /// <summary>
        /// Wzorce czekające na usunięcie z BD
        /// </summary>
        private List<MovementPattern> patternsToRemove = new List<MovementPattern>();

        /// <summary>
        /// Kontekst bazy danych
        /// </summary>
        private RecoilAssistContext PatternContext;

        private bool initialized = false;

        public void Init()
        {
            /*  Tworzy kontekst bazy danych
             *      UWAGA: W tym momencie program łączy się z BD,
             *      oraz tworzony są tabele w BD (jeżeli nie istnieją)
             */
            PatternContext = new RecoilAssistContext();
            // dodaj wzorce z BD do DatabaseProvider'a
            foreach (var item in PatternContext.Patterns)
            {
                var pattern = item.ToPattern();
                patternsInDatabase.Add(pattern);
                patternsBindingList.Add(pattern);
            }
            // "Nasłuchuj" każdą zmianę na liście
            patternsBindingList.ListChanged += PatternsBindingList_ListChanged;
            initialized = true;
        }

        private void PatternsBindingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            MovementPattern item;
            switch (e.ListChangedType)
            {
                // Rekord został dodany => zarejestruj zmianę
                case ListChangedType.ItemAdded:
                    // item - wzorzec dodany
                    item = patternsBindingList[e.NewIndex];

                    // jeśli w BD go nie ma, dodaj go później
                    if (!patternsInDatabase.Contains(item))
                    {
                        patternsToAdd.Add(item);
                    }

                    // jeśli użytkownik chciał go usunąć, ale się rozmyślił => usuń z listy "do usunięcia"
                    else if (patternsToRemove.Contains(item))
                    {
                        patternsToRemove.Remove(item);
                    }
                    break;

                /*  W dwóch przypadkach:
                 *      - ItemDeleted - element został usunięty
                 *      - Reset - nastąpiły "poważne" zmiany w liście
                 *  Nie jesteśmy w stanie klasycznie zarejestrować zmian
                 *  i konieczne jest przeiterowanie całej listy.
                 *  (w przypadku usunięcia elementu, straciliśmy referencję)
                 */
                case ListChangedType.ItemDeleted:
                case ListChangedType.Reset:
                    patternsToAdd.Clear();
                    patternsToRemove.Clear();

                    // dodaj wzorce których nie ma w BD
                    foreach (var pattern in patternsBindingList)
                    {
                        if (!patternsInDatabase.Contains(pattern))
                            patternsToAdd.Add(pattern);
                    }

                    // usuń wzorce których nie ma na liście (a są w BD)
                    foreach (var pattern in patternsInDatabase)
                    {
                        if (!patternsBindingList.Contains(pattern))
                            patternsToRemove.Add(pattern);
                    }
                    break;
            }
        }

        /// <summary>
        /// <see cref="IDatabaseProvider.Patterns"/>
        /// </summary>
        public IList<MovementPattern> Patterns
        {
            get
            {
                if (!initialized)
                    Init();
                return patternsBindingList;
            }
        }

        /// <summary>
        /// <see cref="IDatabaseProvider.CommitChanges"/>
        /// </summary>
        public void CommitChanges()
        {
            // dodaje wzorce z listy "do dodania"
            foreach (var item in patternsToAdd)
                PatternContext.Patterns.Add(item.ToModel());

            // usuwa wzorce z listy "do usunięcia"
            foreach (var item in patternsToRemove)
                PatternContext.Patterns.Remove(item.ToModel());

            // zapisz zmiany
            PatternContext.SaveChanges();

            // przeładuj listy
            patternsToAdd.Clear();
            patternsToRemove.Clear();
            patternsInDatabase.Clear();

            foreach (var item in PatternContext.Patterns)
                patternsInDatabase.Add(item.ToPattern());
        }

        public void Dispose()
        {
            PatternContext.Dispose();
        }
    }
}
