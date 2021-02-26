using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server.Models;

namespace Server.StatisticStorage
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
    }
}
