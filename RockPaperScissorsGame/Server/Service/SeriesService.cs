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
    public class SeriesService:ISeriesService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<TimeOptions> _timeOptions;
        private Series _waiserSeries = null;   
        private static readonly object ob = new object();
        public SeriesService(IMemoryCache memoryCache,IOptions<TimeOptions> timeOptions)
        {
            _memoryCache = memoryCache;
            _timeOptions = timeOptions;
        }

        public void Check()
        {
            _memoryCache.TryGetValue("", out _);
        }

        public Series GetSeries(string key)
        {
           _memoryCache.TryGetValue(key, out var seriesValue );
           return (Series)seriesValue;
        }

        public Series AddToSeries(string user)
        {
            lock (ob)
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
                   var options = new MemoryCacheEntryOptions()
                       .SetSlidingExpiration(_timeOptions.Value.SeriesTimeOut)
                       .RegisterPostEvictionCallback((key, value, reason, substate) =>
                       {
                           ((Series)value).IsDeleted = true;
                       });
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
    }
}
