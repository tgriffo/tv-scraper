using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace datastore.model
{
    public class TvShow
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CastMember> Cast { get; set; }
    }
}