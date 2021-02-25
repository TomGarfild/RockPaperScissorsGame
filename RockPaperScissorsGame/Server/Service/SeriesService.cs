using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Server.Model;
using Server.Options;

namespace Server.Service
{
    public class SeriesService : ISeriesService
    {
        private readonly ConcurrentDictionary<string, string> _privateCode = new ConcurrentDictionary<string, string>();
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<TimeOptions> _timeOptions;
        private Series _waiserSeries = null;
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
                if (_waiserSeries != null)
                {
                    _waiserSeries.AddUser(user);
                    var room = _waiserSeries;
                    _waiserSeries = null;
                    return room;
                }
                else
                {
                    var room = new Series(user);
                    _memoryCache.Set(room.Id, room, options);
                    _waiserSeries = room;
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
    }
}
