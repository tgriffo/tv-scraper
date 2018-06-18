
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace job
{
    public interface ITvMazeApiRepository
    {
        IEnumerable<TvMazeTvShow> GetTvShows();
    }

    [DataContract(Name="shows")]
    public class TvMazeTvShow
    {
        [DataMember(Name="id")]
        public int Id { get; set; }
        
        [DataMember(Name="name")]
        public string Name { get; set; }
    }
}