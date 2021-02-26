using System.Threading;
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
            seriesService.Setup(s => s.SeriesIs(seriesKey)).Returns(true);
            var roundService = new RoundService(seriesService.Object);

            var token1= roundService.StartRound(user1,seriesKey,"Rock");
            var token2 = roundService.StartRound(user2, seriesKey, "Paper");
            roundService.GetResult(user1, seriesKey);
            roundService.GetResult(user2, seriesKey);
            token1 = roundService.StartRound(user1, seriesKey, "Rock");
            var cancel = token1;
            token2 = roundService.StartRound(user2, seriesKey, "Paper");
            var isCancel =((CancellationToken) cancel).IsCancellationRequested; 
            Assert.True(isCancel);
            Assert.Null(token2);
            Assert.True(series.IsFull);
            Assert.False(series.IsDeleted);
        }
    }
}
