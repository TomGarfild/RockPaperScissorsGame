using System.Threading;
using Microsoft.Extensions.Logging;
using Server.Models;

namespace Server.Services
{
    public class RoundService:IRoundService
    {
        private readonly ISeriesService _seriesService;
        private readonly ILogger<RoundService> _iLogger;

        public RoundService(ISeriesService seriesService, ILogger<RoundService> iLogger)
        {
            _seriesService = seriesService;
            _iLogger = iLogger;
        }

        public CancellationToken? StartRound(string user, string seriesKey, string choice)
        {
            var series = _seriesService.GetSeries(seriesKey);
            var user1 = series.Users[0];
            var user2 = series.Users[1];
            if (user == user1)
            {
                series.SetChoice1(choice);
                _iLogger.LogInformation($"User {user1} choice: {choice}");
            }
            else if (user == user2)
            {
                series.SetChoice2(choice);
                _iLogger.LogInformation($"User {user1} choice: {choice}");
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
            {
                var res = _seriesService.GetSeries(seriesKey).GetResult(user);
                _iLogger.LogInformation($"User {user}: {res.ToString()}");
                return res;
            }
            else
                return Round.Result.Undefine;
        }
        public void StartRoundTraining(string user, string seriesKey, string choice)
        {
            var series =(TrainingSeries) _seriesService.GetSeries(seriesKey);
            _iLogger.LogInformation($"User {user} choice: {choice}");
            series.SetChoice1(choice);
            series.SetRandomChoice();
        }
    }
}
