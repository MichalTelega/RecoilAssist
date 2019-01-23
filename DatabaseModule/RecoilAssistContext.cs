using RecoilAssist.Patterns;
using System.Collections.Generic;
using System.Data.Entity;

namespace RecoilAssist.DatabaseModule
{
    /// <summary>
    /// Kontekst bazy danych EntityFramework
    /// </summary>
    public class RecoilAssistContext : DbContext
    {
        public DbSet<PatternModel> Patterns { get; set; }
    }
}
