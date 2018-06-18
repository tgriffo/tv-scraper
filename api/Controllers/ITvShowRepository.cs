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
            var tvShows = _context.TvShows
                .Include(t => t.Cast)
                .OrderBy(t => t.Id)
                .ToList();
            
            tvShows.ForEach(t => t.Cast = t.Cast.OrderByDescending(c => c.Birthday).ToList());
            return tvShows;
        }
    }
}