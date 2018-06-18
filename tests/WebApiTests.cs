using System;
using Xunit;
using System.Collections.Generic;
using api.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using datastore.model;

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

            var list = new List<TvShow>((IEnumerable<TvShow>)results.Value);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public void TestApiWithPaginationWhenMoreThanTenTvShowsAndNoPageParameter()
        {
            var tvshows = GetMockedListOfTvShows(25);
            var mockRepo = new Mock<ITvShowRepository>();
            mockRepo.Setup(repo => repo.Get()).Returns(tvshows);
            
            var controller = new TvShowController(mockRepo.Object);

            var results = Assert.IsType<OkObjectResult>(controller.Get().Result);
            Assert.Equal(200, results.StatusCode);

            var list = new List<TvShow>((IEnumerable<TvShow>)results.Value);
            Assert.Equal(10, list.Count);

            Assert.Equal(1, list[0].Id);
            Assert.Equal(10, list[9].Id);
        }

        [Fact]
        public void TestApiWithPaginationWhenMoreThanTenTvShowsPageOneEqualsNoPageParameter()
        {
            var tvshows = GetMockedListOfTvShows(25);
            var mockRepo = new Mock<ITvShowRepository>();
            mockRepo.Setup(repo => repo.Get()).Returns(tvshows);
            
            var controller = new TvShowController(mockRepo.Object);

            var resultsNoPageParameter = Assert.IsType<OkObjectResult>(controller.Get().Result);
            Assert.Equal(200, resultsNoPageParameter.StatusCode);

            var resultsPageOne = Assert.IsType<OkObjectResult>(controller.Get(1).Result);
            Assert.Equal(200, resultsPageOne.StatusCode);

            var listNoPageParameter = new List<TvShow>((IEnumerable<TvShow>)resultsNoPageParameter.Value);
            var listPageOne = new List<TvShow>((IEnumerable<TvShow>)resultsPageOne.Value);
            Assert.Equal(listNoPageParameter, listPageOne);
        }

        [Fact]
        public void TestApiWithPaginationOnLastPageWithElements()
        {
            var tvshows = GetMockedListOfTvShows(25);
            var mockRepo = new Mock<ITvShowRepository>();
            mockRepo.Setup(repo => repo.Get()).Returns(tvshows);
            
            var controller = new TvShowController(mockRepo.Object);

            var results = Assert.IsType<OkObjectResult>(controller.Get(3).Result);
            Assert.Equal(200, results.StatusCode);

            var list = new List<TvShow>((IEnumerable<TvShow>)results.Value);
            Assert.Equal(5, list.Count);

            Assert.Equal(21, list[0].Id);
            Assert.Equal(25, list[4].Id);
        }

        [Fact]
        public void TestApiWithPaginationOnPageWithNoElementsAndEmptyListReturn()
        {
            var tvshows = GetMockedListOfTvShows(25);
            var mockRepo = new Mock<ITvShowRepository>();
            mockRepo.Setup(repo => repo.Get()).Returns(tvshows);
            
            var controller = new TvShowController(mockRepo.Object);

            var results = Assert.IsType<OkObjectResult>(controller.Get(4).Result);
            Assert.Equal(200, results.StatusCode);

            var list = new List<TvShow>((IEnumerable<TvShow>)results.Value);
            Assert.Empty(list);
        }

        private List<TvShow> GetMockedListOfTvShows(int count)
        {
            var tvshows = new List<TvShow>();
            for (int i = 1; i <= count; i++)
            {
                tvshows.Add(new TvShow() { Id = i, Name = "Tv Show" + i });
            }
            return tvshows;
        }
    }
}
