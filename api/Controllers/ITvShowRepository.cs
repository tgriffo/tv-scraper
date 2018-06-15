using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public interface ITvShowRepository
    {
        IEnumerable<TvShow> Get();
    }

    public class TvShowRepository : ITvShowRepository
    {
        private TvShowContext _context;
        public TvShowRepository(TvShowContext context)
        {
            _context = context;
        }

        public IEnumerable<TvShow> Get()
        {
            return _context.TvShows.AsEnumerable();
        }
    }

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

    public class TvShow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CastMember> Cast { get; set; }
    }

    public class CastMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public TvShow TvShow { get; set; }
    }
}