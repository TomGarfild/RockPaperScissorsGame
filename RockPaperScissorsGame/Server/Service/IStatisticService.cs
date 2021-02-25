using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Server.Model;
using Server.StatisticStorege;

namespace Server.Service
{
    public interface IStatisticService
    {
        public void Add(string login, TimeSpan length, DateTimeOffset time, Round.Result result,
            Round.OptionChoice choice);
        public string GetStatisticItems(string login);
        public string GetGlobalStatistic();
    }
}
