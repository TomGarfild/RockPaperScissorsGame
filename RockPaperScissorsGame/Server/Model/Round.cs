using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Model
{
    public class Round
    {
        public enum OptionChoice
        {
            Undefine = 0,
            Rock = 1,
            Paper = 2,
            Scissor = 3
        }
        public enum Result
        {
            Undefine = 0,
            Lose = 1,
            Draw = 2,
            Win = 3
        }
    }
}
