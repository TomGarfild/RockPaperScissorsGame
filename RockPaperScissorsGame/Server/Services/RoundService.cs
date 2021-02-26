using System.Threading;
using Server.Models;

namespace Server.Services
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
            var user1 = series.Users[0];
            var user2 = series.Users[1];
            if (user == user1)
            {
                series.SetChoice1(choice);
            }
            else if (user == user2)
            {
                series.SetChoice2(choice);
            }

            if (!series.IsRoundDone())
            {
                return series.ReturnToken();
            }
            else
            {
                return null;
            }
        }

        public Round.Result GetResult(string user, string seriesKey)
        {
            if (_seriesService.SeriesIs(seriesKey) && _seriesService.GetSeries(seriesKey).IsRoundDone())
                return _seriesService.GetSeries(seriesKey).GetResult(user);
            else
                return Round.Result.Undefine;
        }
        public void StartRoundTraining(string user, string seriesKey, string choice)
        {
            var series =(TrainingSeries) _seriesService.GetSeries(seriesKey);
            series.SetChoice1(choice);
            series.SetRandomChoice();
        }
    }
}
