using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using api.Controllers;
using datastore.model;

namespace tests
{
    public class RepositoryTests
    {
        private void PopulateTestDatabase(DbContextOptions<TvShowContext> options, int numberOfTvShowsToPopulate, int numberOfCastMembersPerTvShow = 0)
        {
            using (var context = new TvShowContext(options))
            {
                for (int iTvShow = 1; iTvShow <= numberOfTvShowsToPopulate; iTvShow++)
                {
                    var tvShow = new TvShow() { Id = iTvShow, Name = "Tv Show" + iTvShow };
                    var cast = new List<CastMember>();
                    for (int iCast = 1; iCast <= numberOfCastMembersPerTvShow; iCast++)
                    {
                        var id = Int32.Parse(iTvShow.ToString() + iCast.ToString());
                        var member = new CastMember() 
                        { 
                            Id = id, 
                            Name = "Cast " + iCast,
                            Birthday = DateTime.Now.AddYears(-((numberOfCastMembersPerTvShow - iCast) * 10)),
                            TvShow = tvShow
                        };
                        context.CastMembers.Add(member);
                        cast.Add(member);
                    }
                    tvShow.Cast = cast;
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
                var tvShowsCount = context.TvShows.Count();
                Assert.Equal(15, tvShowsCount);
            }
        }

        [Fact]
        public void TestGetReturnsAllTvShowsInDatabase()
        {
            var options = BuildContextOptions("TestRepositoryGetReturnsAllTvShowsInDatabase");
            PopulateTestDatabase(options, 15);

            var context = new TvShowContext(options);

            var repository = new TvShowRepository(context);

            var tvShows = repository.Get();
            var tvShowsCount = tvShows.Count();

            Assert.Equal(15, tvShowsCount);
        }

        [Fact]
        public void TestGetReturnsCastWithTvShows()
        {
            var options = BuildContextOptions("TestGetReturnsCastWithTvShows");
            PopulateTestDatabase(options, 1, 1);

            var context = new TvShowContext(options);

            var repository = new TvShowRepository(context);

            var tvShows = repository.Get();

            Assert.NotEmpty(tvShows.ToArray()[0].Cast);
        }

        [Fact]
        public void TestGetReturnCastInDescendingBirthdayOrder()
        {
            var options = BuildContextOptions("TestGetReturnCastInDescendingBirthdayOrder");
            PopulateTestDatabase(options, 1, 3);

            var context = new TvShowContext(options);

            var repository = new TvShowRepository(context);

            var tvShows = repository.Get();
            var cast = tvShows.ToArray()[0].Cast;

            Assert.True(cast[0].Birthday > cast[1].Birthday);
            Assert.True(cast[1].Birthday > cast[2].Birthday);
        }
    }
}
