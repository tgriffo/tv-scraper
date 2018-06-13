using System;
using System.Collections.Generic;

namespace api.Controllers
{
    public interface ITvShowRepository
    {
        IEnumerable<TvShow> Get();
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
    }
}