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
        private void PopulateTestDatabase(DbContextOptions<TvShowContext> options)
        {
            using (var context = new TvShowContext(options))
            {
                for (int i = 1; i <= 15; i++)
                {
                    var tvShow = new TvShow() { Id = i, Name = "Tv Show" + i, Cast = new List<CastMember>() };
                    context.TvShows.Add(tvShow);
                }

                context.SaveChanges();
            }
        }

        [Fact]
        public void TestInMemoryDatabaseSetupWorks()
        {
            var options = new DbContextOptionsBuilder<TvShowContext>()
                .UseInMemoryDatabase(databaseName: "Test_database")
                .Options;

            PopulateTestDatabase(options);

            using (var context = new TvShowContext(options))
            {
                Assert.Equal(15, context.TvShows.Count());
            }
        }
    }
}
