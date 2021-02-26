using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<SeriesService> _iLogger;
        private Series _waitSeries = null;
        private MemoryCacheEntryOptions options = null;
        private static readonly object Ob = new object();

        public SeriesService(IMemoryCache memoryCache, IOptions<TimeOptions> timeOptions,ILogger<SeriesService> iLogger)
        {
            _memoryCache = memoryCache;
            _timeOptions = timeOptions;
            _iLogger = iLogger;
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
                    _iLogger.LogInformation($"Add to series:{room.Id} with user:{user}");
                    return room;
                }
                else
                {
                    var room = new Series(user);
                    _memoryCache.Set(room.Id, room, options);
                    _waitSeries = room;
                    _iLogger.LogInformation($"Create series:{room.Id} with user:{user}");
                    return room;
                }
            }
        }

        public bool SeriesIs(string key)
        {
            return _memoryCache.TryGetValue(key, out var series) && (!((Series)series).IsDeleted);
        }

        public PrivateSeries AddToPrivateSeries(string user)
        {

            var series = PrivateSeries.GetNewPrivateSeries();
            _memoryCache.Set(series.Id, series, options);
            _privateCode.TryAdd(series.Code, series.Id);
            _iLogger.LogInformation($"Create private series:{series.Id}");
            return series;

        }

        public PrivateSeries SearchAndAddToPrivateSeries(string user, string code)
        {
            if (_privateCode.ContainsKey(code))
            {
                var series = (PrivateSeries)_memoryCache.Get(_privateCode[code]);
                if (!series.IsFull)
                {
                    series.AddUser(user);
                    _iLogger.LogInformation($"Add to private series:{series.Id} with user:{user}");
                    return series;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public TrainingSeries AddToTrainingSeries(string user)
        {
            var series = new TrainingSeries(user);
            _memoryCache.Set(series.Id, series, options);
            _iLogger.LogInformation($"Create traning series:{series.Id} with user:{user}");
            return series;
        }

        public void CancelSeries(string series)
        {
            if (_waitSeries.Id == series)
            {
                _waitSeries.IsDeleted = true;
                _waitSeries.CancelRound();
                _waitSeries = null;
            }
            if(SeriesIs(series))
                _memoryCache.Remove(series);
            _iLogger.LogInformation($"Cancel series:{series}");
        }
    }
}
