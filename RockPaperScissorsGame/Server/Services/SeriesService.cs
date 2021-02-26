using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Server.Models;
using Server.Options;

namespace Server.Services
{
    public class SeriesService : ISeriesService
    {
        private readonly ConcurrentDictionary<string, string> _privateCode = new ConcurrentDictionary<string, string>();
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<TimeOptions> _timeOptions;
        private Series _waitSeries = null;
        private MemoryCacheEntryOptions options = null;
        private static readonly object Ob = new object();

        public SeriesService(IMemoryCache memoryCache, IOptions<TimeOptions> timeOptions)
        {
            _memoryCache = memoryCache;
            _timeOptions = timeOptions;
            options = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(_timeOptions.Value.SeriesTimeOut)
                .RegisterPostEvictionCallback((key, value, reason, substate) =>
                {
                    ((Series)value).IsDeleted = true;
                });
        }

        public void Check()
        {
            _memoryCache.TryGetValue("", out _);
        }

        public Series GetSeries(string key)
        {
            _memoryCache.TryGetValue(key, out var seriesValue);
            return (Series)seriesValue;
        }

        public Series AddToPublicSeries(string user)
        {
            lock (Ob)
            {
                if ((_waitSeries != null)&&(!_waitSeries.Users.Contains(user)))
                {
                    _waitSeries.AddUser(user);
                    var room = _waitSeries;
                    _waitSeries = null;
                    return room;
                }
                else
                {
                    var room = new Series(user);
                    _memoryCache.Set(room.Id, room, options);
                    _waitSeries = room;
                    return room;
                }
            }
        }

        public bool SeriesIs(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public PrivateSeries AddToPrivateSeries(string user)
        {

            var series = PrivateSeries.GetNewPrivateSeries();
            _memoryCache.Set(series.Id, series, options);
            _privateCode.TryAdd(series.Code, series.Id);
            return series;

        }

        public PrivateSeries SearchAndAddToPrivateSeries(string user, string code)
        {
            var series = (PrivateSeries)_memoryCache.Get(_privateCode[code]);
            series.AddUser(user);
            return series;
        }

        public TrainingSeries AddToTrainingSeries(string user)
        {
            var series = new TrainingSeries(user);
            _memoryCache.Set(series.Id, series, options);
            return series;
        }

        public void CancelSeries(string series)
        {
            if (_waitSeries.Id == series)
                _waitSeries = null;
            if(SeriesIs(series))
                _memoryCache.Remove(series);
        }
    }
}
