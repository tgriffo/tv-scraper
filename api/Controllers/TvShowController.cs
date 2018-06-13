using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/tv-shows")]
    [ApiController]
    public class TvShowController : ControllerBase
    {
        private ITvShowRepository _tvShowRepository;
        private const int _numberOfObjectsPerPage = 10;

        public TvShowController(ITvShowRepository tvShowRepository)
        {
            _tvShowRepository = tvShowRepository;
        }

        // GET api/tv-shows
        [HttpGet]
        public ActionResult<IEnumerable<TvShow>> Get()
        {
            return Ok(GetWithPagination(0));
        }

        private IEnumerable<TvShow> GetWithPagination(int pageIndex)
        {
            var shows = _tvShowRepository.Get();

            return shows
                .Skip(_numberOfObjectsPerPage * pageIndex)
                .Take(_numberOfObjectsPerPage);
        }
    }
}
