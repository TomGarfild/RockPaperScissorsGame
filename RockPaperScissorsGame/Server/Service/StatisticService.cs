using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Server.Model;
using Server.StatisticStorege;

namespace Server.Service
{
    public class StatisticService:IStatisticService
    {
        private readonly StatisticContext _statisticContext;

        public StatisticService(StatisticContext statisticContext)
        {
            _statisticContext = statisticContext;
        }

        public async void Add(string login, TimeSpan length, DateTimeOffset time, Round.Result result, Round.OptionChoice choice)
        {
            await _statisticContext.StatisticItems.AddAsync(new StatisticItem(login, result, length, time, choice));
            await _statisticContext.SaveChangesAsync();
        }

        public  IEnumerable<StatisticItem> GetStatisticItems(string login)
        {
            return _statisticContext.StatisticItems.Where(s => s.Login == login);
        }

        public IEnumerable<StatisticItem> GetGlobalStatistic()
        {
            return _statisticContext.StatisticItems.GroupBy(s => s.Login).Where(s => s.Count() >= 10).SelectMany(s=>s);
        }
    }
}
