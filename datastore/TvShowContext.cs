using Microsoft.EntityFrameworkCore;
using datastore.model;

namespace datastore
{
    public class TvShowContext : DbContext
    {
        public TvShowContext()
        { }

        public TvShowContext(DbContextOptions<TvShowContext> options)
            : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("TODO");
            }
        }

        public DbSet<TvShow> TvShows { get; set; }
        public DbSet<CastMember> CastMembers { get; set; }
    }
}