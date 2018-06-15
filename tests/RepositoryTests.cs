using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using api.Controllers;

namespace tests
{
    public class RepositoryTests
    {
        private void PopulateTestDatabase(DbContextOptions<TvShowContext> options, int numberOfTvShowsToPopulate)
        {
            using (var context = new TvShowContext(options))
            {
                for (int i = 1; i <= numberOfTvShowsToPopulate; i++)
                {
                    var tvShow = new TvShow() { Id = i, Name = "Tv Show" + i, Cast = new List<CastMember>() };
                    context.TvShows.Add(tvShow);
                }

                context.SaveChanges();
            }
        }

        private DbContextOptions<TvShowContext> BuildContextOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<TvShowContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
        }

        [Fact]
        public void TestInMemoryDatabaseSetupWorks()
        {
            var options = BuildContextOptions("TestInMemoryDatabaseSetupWorks");
            PopulateTestDatabase(options, 15);

            using (var context = new TvShowContext(options))
            {
                Assert.Equal(15, context.TvShows.Count());
            }
        }

        [Fact]
        public void TestRepositoryGetReturnsAllTvShowsInDatabase()
        {
            var options = BuildContextOptions("TestRepositoryGetReturnsAllTvShowsInDatabase");
            PopulateTestDatabase(options, 15);

            var context = new TvShowContext(options);

            var repository = new TvShowRepository(context);

            var tvShows = repository.Get();
            var tvShowsCount = tvShows.Count();

            Assert.Equal(15, tvShowsCount);
        }
    }
}
