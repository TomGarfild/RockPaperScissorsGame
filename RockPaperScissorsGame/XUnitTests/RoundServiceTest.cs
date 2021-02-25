using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using Server.Model;
using Server.Service;
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
            var user = "aaa";
            var series = new Series(user);
            seriesService.Setup(s => s.GetSeries(seriesKey)).Returns(series);
            var roundService = new RoundService(seriesService.Object);

            //roundService.StartRound(user,seriesKey,Round.OptionChoice.Rock);

            Assert.Equal("aaa", series.Users[0]);
            Assert.False(series.IsFull);
            Assert.False(series.IsDeleted);
        }
    }
}
