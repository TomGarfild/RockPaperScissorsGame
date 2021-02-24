using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.Controllers;
using Server.Model;

namespace Server.Service
{
    public interface ISeriesService
    {
        public Series AddToSeries(string user);
        public void Check();
        public bool SeriesIs(string key);
        public Series GetSeries(string key);
    }
}
