using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using job;
using datastore;
using datastore.model;

namespace tests
{
    public class JobTests
    {
        [Fact]
        public void TestFetchsTvShowsFromTvMazeApi()
        {
            var repository = new TvMazeApiRepository();

            var tvShows = repository.GetTvShows();

            Assert.NotEmpty(tvShows);
        }

        [Fact]
        public void TestPopulatesTvShowsInDatastore()
        {
            var tvShows = new List<TvMazeTvShow>()
            {
                new TvMazeTvShow() { Id = 1, Name = "Name 1" },
                new TvMazeTvShow() { Id = 2, Name = "Name 2" }
            };
            var mockRepo = new Mock<ITvMazeApiRepository>();
            mockRepo.Setup(repo => repo.GetTvShows()).Returns(tvShows);

            var options = BuildContextOptions("TestPopulatesTvShowsInDatastore");

            using (var context = new TvShowContext(options))
            {
                var populator = new DataPopulator(mockRepo.Object, context);
                var result = populator.PopulateTvShows();

                Assert.Equal("success", result);
                Assert.NotEmpty(context.TvShows);

                Assert.Equal(1, context.TvShows.ToList()[0].Id);
                Assert.Equal("Name 1", context.TvShows.ToList()[0].Name);

                Assert.Equal(2, context.TvShows.ToList()[1].Id);
                Assert.Equal("Name 2", context.TvShows.ToList()[1].Name);
            }
        }

        [Fact]
        public void TestUpdatesWhenTvShowAlreadyExistsInTheDatabase()
        {
            var tvShows = new List<TvMazeTvShow>()
            {
                new TvMazeTvShow() { Id = 1, Name = "Name 1" },
                new TvMazeTvShow() { Id = 2, Name = "Name 2" }
            };
            var mockRepo = new Mock<ITvMazeApiRepository>();
            mockRepo.Setup(repo => repo.GetTvShows()).Returns(tvShows);

            var options = BuildContextOptions("TestUpdatesWhenTvShowAlreadyExistsInTheDatabase");
            
            using (var context = new TvShowContext(options))
            {
                context.TvShows.AddRange(new List<TvShow>()
                {
                    new TvShow() { Id = 1, Name = "Old Name 1" },
                    new TvShow() { Id = 2, Name = "Old Name 2" }
                });

                context.SaveChanges();
            }

            using (var context = new TvShowContext(options))
            {
                var populator = new DataPopulator(mockRepo.Object, context);
                var result = populator.PopulateTvShows();

                Assert.Equal("success", result);
                Assert.NotEmpty(context.TvShows);

                Assert.Equal(1, context.TvShows.ToList()[0].Id);
                Assert.Equal("Name 1", context.TvShows.ToList()[0].Name);

                Assert.Equal(2, context.TvShows.ToList()[1].Id);
                Assert.Equal("Name 2", context.TvShows.ToList()[1].Name);
            }
        }

        private DbContextOptions<TvShowContext> BuildContextOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<TvShowContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
        }
    }
}