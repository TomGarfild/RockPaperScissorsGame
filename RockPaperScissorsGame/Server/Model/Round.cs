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

        private OptionChoice _user1Choice = OptionChoice.Undefine;
        private OptionChoice _user2Choice = OptionChoice.Undefine;
        public CancellationTokenSource Source { get; private set; } = new CancellationTokenSource();

        public Result GetResult()
        {
            if (_user1Choice == _user2Choice)
                return Result.Draw;
            else if (_user1Choice == OptionChoice.Rock && _user2Choice == OptionChoice.Scissor)
            {
                return Result.Win;
            }
            else if (_user1Choice > _user2Choice)
            {
                return Result.Win;
            }

            return Result.Lose;
        }

        public void SetChoice1(string choice)
        {
            _user1Choice = ParseChoice(choice);
        }
        public void SetChoice2(string choice)
        {
            _user2Choice = ParseChoice(choice);
        }
        public void Clear()
        {
            Source.Cancel();
            _user1Choice = Round.OptionChoice.Undefine;
            _user2Choice = Round.OptionChoice.Undefine;
            Source = new CancellationTokenSource();
        }
        public static OptionChoice ParseChoice(string choice)
        {
            switch (choice)
            {
                case "Rock":
                    return OptionChoice.Rock;
                case "Paper":
                    return OptionChoice.Paper;
                case "Scissors":
                    return OptionChoice.Scissor;
                default:
                    return OptionChoice.Undefine;
            }
        }

        public void SetRandomChoice()
        {
            _user2Choice =(OptionChoice) new Random().Next(3) + 1;
        }
    }
}
