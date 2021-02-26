using Server.Models;

namespace Server.Services
{
    public interface ISeriesService
    {
        public Series AddToPublicSeries(string user);
        public PrivateSeries AddToPrivateSeries(string user);
        public PrivateSeries SearchAndAddToPrivateSeries(string user,string code);
        public TrainingSeries AddToTrainingSeries(string user);
        public void Check();
        public bool SeriesIs(string key);
        public Series GetSeries(string key);
        public void CancelSeries(string series);
    }
}
