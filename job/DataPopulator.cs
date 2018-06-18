using System;
using System.Collections.Generic;
using System.Linq;

using datastore;
using datastore.model;

namespace job
{
    public class PopulateResult
    {
        public int Added { get; set; }
        public int Changed { get; set; }
        public bool Successful { get; set; }
        public string Error { get; set; }
    }

    public class DataPopulator
    {
        private ITvMazeApiRepository _mazeApiRepository;
        private TvShowContext _db;

        public DataPopulator(ITvMazeApiRepository mazeApiRepository, TvShowContext db)
        {
            _mazeApiRepository = mazeApiRepository;
            _db = db;
        }

        public PopulateResult PopulateTvShows()
        {
            var result = new PopulateResult() { Successful = true };

            try
            {
                var tvShowsMazeApi = _mazeApiRepository.GetTvShows();
                foreach (var tvShow in tvShowsMazeApi.Select(t => new TvShow() { Id = t.Id, Name = t.Name }))
                {
                    var existingTvShow = _db.TvShows.Find(tvShow.Id);
                    if (existingTvShow == null)
                    {
                        result.Added++;
                        _db.TvShows.Add(tvShow);
                    }
                    else
                    {
                        result.Changed++;
                        existingTvShow.Name = tvShow.Name;
                    }
                }
                
                _db.SaveChanges();
            }
            catch (TvMazeApiException e)
            {
                result.Added = 0;
                result.Changed = 0;
                result.Successful = false;
                result.Error = e.Message;
            }
            return result;
        }
    }
}