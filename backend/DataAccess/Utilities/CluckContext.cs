using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Utilities
{
    public class CluckContext : DbContext
    {
        private CluckContextFactory _contextFactory;
        private SqliteConnection _connection;

        public CluckContext() { }

        public CluckContext(CluckContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public CluckContext(DbContextOptions<CluckContext> options, CluckContextFactory contextFactory) : base(options)
        {
            _contextFactory = contextFactory;
        }

        public CluckContext(DbContextOptions<CluckContext> options, CluckContextFactory contextFactory,
            SqliteConnection connection) : this(options, contextFactory)
        {
            _connection = connection;
        }
    }
}
