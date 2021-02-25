using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public  string GetStatisticItems(string login)
        {

            var list = _statisticContext.StatisticItems.Where(s => s.Login == login).ToList();

            var str = new StringBuilder($"Login: {list[0].Login}\n");
            var time = new TimeSpan();
            list.Select(l => time += l.Length);
            str.AppendLine($"Total time: {time}");
            str.AppendLine($"Total Game: {list.Count}");
            var win = list.Count(l => l.Result == Round.Result.Win);
            str.AppendLine($"Part Win: {win * 100 / list.Count}%");
            var winToday = list.Count(l =>
                ((l.Result == Round.Result.Win) && (l.Time.Day == DateTimeOffset.Now.Day) &&
                 (l.Time.Month == DateTimeOffset.Now.Month) && (l.Time.Year == DateTimeOffset.Now.Year)));
            str.AppendLine($"Today Win: {winToday}");
            return str.ToString();
        }

        public string GetGlobalStatistic()
        {
            var list = _statisticContext.StatisticItems.Where(s=>s.Result ==Round.Result.Win).ToList();
            var dic  = new Dictionary<string,int>();
            list.ForEach(l =>
            {
                if (dic.ContainsKey(l.Login))
                {
                    dic[l.Login] += 1;
                }
                else
                {
                    dic.TryAdd(l.Login, 1);
                }
            });
            var str = new StringBuilder("");
            str.AppendLine($"\tLogin\tWin");

            dic.OrderBy(d=>d.Value).Select(l =>
            {
                if (l.Value >= 10)
                {
                    str.Append($"\t{l.Key}\t");
                    str.AppendLine(l.Value.ToString());
                }

                return "";
            });
            return str.ToString();
        }
    }
}
