using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Model;

namespace Server.Service
{
    public interface IRoundService
    {
        public CancellationToken? StartRound(string user, string seriesKey, string choice);
        public Round.Result GetResult(string user, string seriesKey);
    }
}
