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
        public TvShowController(ITvShowRepository tvShowRepository)
        {
            _tvShowRepository = tvShowRepository;
        }

        // GET api/tv-shows
        [HttpGet]
        public ActionResult<IEnumerable<TvShow>> Get()
        {
            return Ok(_tvShowRepository.Get());
        }
    }
}
