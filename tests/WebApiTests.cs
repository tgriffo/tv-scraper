using System;
using Xunit;
using System.Collections.Generic;
using api.Controllers;

namespace tests
{
    public class WebApiTests
    {
        [Fact]
        public void TestApiNoPagination()
        {
            var controller = new TvShowController();
            
            var results = new List<string>(controller.Get().Value);

            Assert.Equal(2, results.Count);
        }
    }
}
