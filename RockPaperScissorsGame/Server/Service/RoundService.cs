using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.Model;

namespace Server.Service
{
    public class RoundService:IRoundService
    {
        private readonly ISeriesService _seriesService;
        public RoundService(ISeriesService seriesService)
        {
            _seriesService = seriesService;
        }

        public CancellationToken? StartRound(string user, string seriesKey, string choice)
        {
            var series = _seriesService.GetSeries(seriesKey);
            var Users = series.Users;
            var user1 = Users[0];
            var user2 = Users[1];
            if (user == user1)
            {
                series.User1Choice = ParseChoice( choice);
            }
            else if (user == user2)
            {
                series.User2Choice = ParseChoice(choice);
            }

            if (series.User1Choice != Round.OptionChoice.Undefine && series.User2Choice != Round.OptionChoice.Undefine)
            {
                series.Source.Cancel();
                return null;
            }

            return series.Source.Token;
        }

        public Round.Result GetResult(string user, string seriesKey)
        {
            var series = _seriesService.GetSeries(seriesKey);
            return series.GetResult(user);
        }

        public Round.OptionChoice ParseChoice(string choice)
        {
            switch (choice)
            {
                case "Rock":
                    return Round.OptionChoice.Rock;
                case "Paper":
                    return Round.OptionChoice.Paper;
                case "Scissor":
                    return Round.OptionChoice.Scissor;
                default:
                    return Round.OptionChoice.Undefine;
        }
        }
    }
}
