using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.StatisticStorage;

namespace Server.Services
{
    public class StatisticService:IStatisticService
    {
        private readonly StatisticContext _statisticContext;
        private readonly ILogger<StatisticService> _iLogger;

        public StatisticService(StatisticContext statisticContext, ILogger<StatisticService> Ilogger)
        {
            _statisticContext = statisticContext;
            _iLogger = Ilogger;
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
            list.ForEach(l => time = time.Add(l.Length));
            str.AppendLine($"Total time: {time.ToString()}");
            str.AppendLine($"Total Game: {list.Count}");
            var win = list.Count(l => l.Result == Round.Result.Win);
            str.AppendLine($"Part Win: {win * 100 / list.Count}%");
            var winToday = list.Count(l =>
                ((l.Result == Round.Result.Win) && (l.Time.Day == DateTimeOffset.Now.Day) &&
                 (l.Time.Month == DateTimeOffset.Now.Month) && (l.Time.Year == DateTimeOffset.Now.Year)));
            str.AppendLine($"Today Win: {winToday}");
            _iLogger.LogInformation($"Get Local stat for {login}");
            return str.ToString();
        }

        public string GetGlobalStatistic()
        {
            var dic  = new Dictionary<string,int>();
            _statisticContext.StatisticItems.ToList().ForEach(l =>
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
            var dicSort = dic.Where(d => d.Value >= 10);
            var str = new StringBuilder("");
            str.AppendLine($"\tLogin\tWin");
            var dic1 = new Dictionary<string, int>();
            _statisticContext.StatisticItems.Where(s=>s.Result== Round.Result.Win).ToList().ForEach(l =>
            {
                if (dicSort.Any(d=>d.Key==l.Login))
                {
                    if (dic1.ContainsKey(l.Login))
                    {
                        dic1[l.Login] += 1;
                    }
                    else
                    {
                        dic1.TryAdd(l.Login, 1);
                    }
                }
            });
            dic1.OrderBy(d=>d.Value).Select(l =>
            {

                str.Append($"\t{l.Key}\t");
                str.AppendLine(l.Value.ToString());
                return "";
            });
            return str.ToString();
        }
    }
}
