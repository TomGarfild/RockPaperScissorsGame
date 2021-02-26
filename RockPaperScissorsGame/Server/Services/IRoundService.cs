using System.Threading;
using Server.Models;

namespace Server.Services
{
    public interface IRoundService
    {
        public CancellationToken? StartRound(string user, string seriesKey, string choice);
        public Round.Result GetResult(string user, string seriesKey);
        public void StartRoundTraining(string user, string seriesKey, string choice);
    }
}
