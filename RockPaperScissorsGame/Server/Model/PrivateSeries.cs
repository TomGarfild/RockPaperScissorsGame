using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
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
            Code = Id.Substring(0, 4);
        }
    }
}
