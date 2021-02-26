using Moq;
using Server.Models;
using Server.Services;
using Xunit;

namespace XUnitTests
{
    public class RoundServiceTest
    {
        [Fact]
        public void TestStartRound()
        {
            var seriesService = new Mock<ISeriesService>();
            var seriesKey = "444444";
            var user1 = "aaa";
            var user2 = "bbb";
            var series = new Series(user1);
            series.AddUser(user2);
            seriesService.Setup(s => s.GetSeries(seriesKey)).Returns(series);
            var roundService = new RoundService(seriesService.Object);

            var token1= roundService.StartRound(user1,seriesKey,"Rock");
            var token2 = roundService.StartRound(user2, seriesKey, "Paper");


            Assert.Null(token2);
            Assert.True(series.IsFull);
            Assert.False(series.IsDeleted);
        }
    }
}
