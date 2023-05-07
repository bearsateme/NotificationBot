using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace DataAccess.Utilities
{
    public class CluckContext : DbContext
    {
        private CluckContextFactory _contextFactory;
        private SqliteConnection _connection;

        public DbSet<GuildTeamEntity> GuildTeam { get; set; }
        
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
