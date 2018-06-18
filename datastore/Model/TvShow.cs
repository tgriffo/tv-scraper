using System.Collections.Generic;

namespace datastore.model
{
    public class TvShow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CastMember> Cast { get; set; }
    }
}