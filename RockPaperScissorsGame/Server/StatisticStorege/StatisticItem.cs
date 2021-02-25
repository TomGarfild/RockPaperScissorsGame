using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Server.Model;

namespace Server.StatisticStorege
{
    public class StatisticItem
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public Round.Result Result { get; set; }
        public TimeSpan Length { get; set; }
        public DateTimeOffset Time { get; set; }
        public Round.OptionChoice Choice { get; set; }

        public StatisticItem(string login, Round.Result result, TimeSpan length, DateTimeOffset time,
            Round.OptionChoice choice)
        {
            Login = login;
            Result = result;
            Length = length;
            Time = time;
            Choice = choice;
        }

        public static string ToStringStatisticLocalPlayer(IList<StatisticItem> list)
        {
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

        public static string ToStringStatisticGlobal(IList<StatisticItem> list)
        {
            var str = new StringBuilder("");
            str.AppendLine($"\tLogin\tWin");
            var dic = new Dictionary<string, int>();
            list.Select(s =>
            {
                if (dic.ContainsKey(s.Login))
                {
                    dic[s.Login] += 1;
                }
                else
                {
                    dic.TryAdd(s.Login, 1);
                }

                return 1;
            });
            dic.OrderBy(d => d.Value).Select(s =>
            {
                str.AppendLine($"\t{s.Key}\t{s.Value}");
                return 1;
            });
            return str.ToString();
        }
    }
}
