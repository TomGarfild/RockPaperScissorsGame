using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;
using Moq;
using Server.Options;
using Server.Services;

namespace XUnitTests
{
    public class SeriesServiceTest
    {
        public IMemoryCache GetMemoryCache()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            return memoryCache;
        }
        public ILogger<T> GetLogger<T>()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<T>>();
            return logger;
        }
        [Fact]
        public void TestCreatePublicSeries()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            var series = seriesService.AddToPublicSeries("aaa");

            Assert.Equal("aaa", series.Users[0]);
            Assert.False(series.IsFull);
            Assert.False(series.IsDeleted);
        }
        [Fact]
        public void TestAddToPublicSeries()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            seriesService.AddToPublicSeries("aaa");
            var series = seriesService.AddToPublicSeries("bbb");

            Assert.Equal("aaa", series.Users[0]);
            Assert.Equal("bbb", series.Users[1]);
            Assert.True(series.IsFull);
            Assert.False(series.IsDeleted);
        }
        [Fact]
        public void TestAddToPrivateSeries()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            var series = seriesService.AddToPrivateSeries("aaa");


            Assert.Empty(series.Users);
            Assert.False(series.IsFull);
            Assert.False(series.IsDeleted);
        }
        [Fact]
        public void TestAddFirstPlayerToPrivateSeries()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            var series = seriesService.AddToPrivateSeries("aaa");
            series = seriesService.SearchAndAddToPrivateSeries("aaa", series.Code);

            Assert.Contains("aaa", series.Users);
            Assert.False(series.IsFull);
            Assert.False(series.IsDeleted);
        }
        [Fact]
        public void TestAddSecondPlayerToPrivateSeries()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            var seriesEmpty = seriesService.AddToPrivateSeries("aaa");
            seriesService.SearchAndAddToPrivateSeries("aaa", seriesEmpty.Code);
            var series = seriesService.SearchAndAddToPrivateSeries("bbb", seriesEmpty.Code);

            Assert.Contains("bbb", series.Users);
            Assert.True(series.IsFull);
            Assert.False(series.IsDeleted);
        }
        [Fact]
        public void TestCreateTrainingSeries()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            var series = seriesService.AddToTrainingSeries("aaa");

            Assert.Contains("aaa", series.Users);
            Assert.True(series.Users.Count == 1);
            Assert.False(series.IsDeleted);
        }

        [Fact]
        public void TestSeriesIsTrue()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            var series = seriesService.AddToPublicSeries("aaa");

            Assert.True(seriesService.SeriesIs(series.Id));
        }
        [Fact]
        public void TestSeriesIsFalse()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());
            Assert.False(seriesService.SeriesIs(Guid.NewGuid().ToString()));
        }
        [Fact]
        public void TestGetSeriesTrue()
        {
            var mockcach = GetMemoryCache();
            var mockOptions = new Mock<IOptions<TimeOptions>>();
            mockOptions.Setup(m => m.Value).Returns(new TimeOptions() { SeriesTimeOut = TimeSpan.FromMinutes(5) });
            var seriesService = new SeriesService(mockcach, mockOptions.Object, GetLogger<SeriesService>());

            var series = seriesService.AddToPublicSeries("aaa");

            Assert.Equal(series, seriesService.GetSeries(series.Id));
        }
    }
}
