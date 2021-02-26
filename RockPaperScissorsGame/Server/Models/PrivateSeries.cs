using System;

namespace Server.Models
{
    public class PrivateSeries :Series
    {
        public string Code { get; set; }
        public PrivateSeries(string user) : base(user)
        {
            Code = Id.Substring(0, 4);
        }

        public PrivateSeries():base()
        {

        }
        public static PrivateSeries GetNewPrivateSeries()
        {
            var series = new PrivateSeries() { IsFull = false, IsDeleted = false, Id = Guid.NewGuid().ToString()};
            series.Code = series.Id.Substring(0, 4);
            return series;

        }
    }
}
