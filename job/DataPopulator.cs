using System;
using System.Collections.Generic;
using System.Linq;

using datastore;
using datastore.model;

namespace job
{
    public class DataPopulator
    {
        private ITvMazeApiRepository _mazeApiRepository;
        private TvShowContext _db;

        public DataPopulator(ITvMazeApiRepository mazeApiRepository, TvShowContext db)
        {
            _mazeApiRepository = mazeApiRepository;
            _db = db;
        }

        public string PopulateTvShows()
        {
            var tvShowsMazeApi = _mazeApiRepository.GetTvShows();
            
            foreach (var tvShow in tvShowsMazeApi.Select(t => new TvShow() { Id = t.Id, Name = t.Name }))
            {
                var existingTvShow = _db.TvShows.Find(tvShow.Id);
                if (existingTvShow == null)
                {
                    _db.TvShows.Add(tvShow);
                }
                else
                {
                    existingTvShow.Name = tvShow.Name;
                }
            }
            
            _db.SaveChanges();
            return "success";
        }
    }
}