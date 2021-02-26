using System;
using Server.Models;

namespace Server.Services
{
    public interface IStatisticService
    {
        public void Add(string login, TimeSpan length, DateTimeOffset time, Round.Result result,
            Round.OptionChoice choice);
        public string GetStatisticItems(string login);
        public string GetGlobalStatistic();
    }
}
