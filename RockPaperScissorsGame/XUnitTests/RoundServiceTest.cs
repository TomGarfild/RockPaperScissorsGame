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

            //roundService.StartRound(user,seriesKey,"Rock");

           // series.Ver
            Assert.False(series.IsFull);
            Assert.False(series.IsDeleted);
        }
    }
}
