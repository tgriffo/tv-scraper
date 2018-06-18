using System;
using System.Collections.Generic;

namespace datastore.model
{
    public class CastMember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public TvShow TvShow { get; set; }
    }
}