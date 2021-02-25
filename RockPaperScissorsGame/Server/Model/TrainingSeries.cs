using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Model
{
    public class TrainingSeries:Series
    {
        public TrainingSeries()
        {

        }
        public TrainingSeries(string user) : base(user)
        {
        }
        public void SetRandomChoice()
        {
            _round.SetRandomChoice();
        }

        public override Round.Result GetResult(string user)
        {
            return _round.GetResult();
        }

    }
}
