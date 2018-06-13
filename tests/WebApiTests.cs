using System;
using Xunit;
using System.Collections.Generic;
using api.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace tests
{
    public class WebApiTests
    {
        [Fact]
        public void TestApiNoPagination()
        {
            var tvshows = new List<TvShow>()
            {
                new TvShow()
                {
                    Id = 1, 
                    Name = "Tv Show 1", 
                    Cast = new List<CastMember>()
                    { 
                        new CastMember() { Id = 1, Name = "Actor 1", Birthday = DateTime.Now.AddYears(-30) }
                    } 
                },
                new TvShow() { Id = 2, Name = "Tv Show 2" }
            };

            var mockRepo = new Mock<ITvShowRepository>();
            mockRepo.Setup(repo => repo.Get()).Returns(tvshows);
            
            var controller = new TvShowController(mockRepo.Object);

            var results = Assert.IsType<OkObjectResult>(controller.Get().Result);
            Assert.Equal(200, results.StatusCode);

            var list = new List<TvShow>(Assert.IsType<List<TvShow>>(results.Value));
            Assert.Equal(2, list.Count);
        }
    }
}
