using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Options
{
    public class TimeOptions
    {
        public TimeSpan SeriesTimeOut { get; set; }
        public TimeSpan RoundTimeOut { get; set; }
    }
}
