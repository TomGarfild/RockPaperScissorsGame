using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Server.Model
{
    public class Series
    {
        private bool _checkResult = false;
        public Round.OptionChoice User1Choice { get; set; } = Round.OptionChoice.Undefine;
        public Round.OptionChoice User2Choice { get; set; } = Round.OptionChoice.Undefine;
        public CancellationTokenSource Source { get; private set; } = new CancellationTokenSource();
        public string Id { get; set; }
        public List<string> Users { get;  } = new List<string>();
        public bool IsFull{get; set; }
        public bool IsDeleted { get; set; }
        public Series(string user)
        {
            Users.Add(user);
            IsFull = false;
            IsDeleted = false;
            Id = Guid.NewGuid().ToString();
        }
        public void AddUser(string user)
        {
            if (!IsFull)
            {
                Users.Add(user);
                IsFull = true;
            }
        }

        public Round.Result GetResult(string user)
        {
            if (Users[0] == user)
            {
                var res = Result(User1Choice, User2Choice);
                if (!_checkResult)
                {
                    _checkResult = true;
                }
                else
                {
                    Clear();
                }
                return res;
            }
            else if (Users[1] == user)
            {
                var res = Result(User2Choice, User1Choice);
                if (!_checkResult)
                {
                    _checkResult = true;
                }
                else
                {
                    Clear();
                }
                return res;
            }
            if (!_checkResult)
            {
                _checkResult = true;
            }
            else
            {
                Clear();
            }
            return Round.Result.Undefine;
        }

        public void Clear()
        {
            User1Choice = Round.OptionChoice.Undefine;
            User2Choice = Round.OptionChoice.Undefine;
            Source = new CancellationTokenSource();
        }
        private Round.Result Result(Round.OptionChoice User1Choice, Round.OptionChoice User2Choice)
        {
            if (User1Choice == User2Choice)
                return Round.Result.Draw;
            else if (User1Choice == Round.OptionChoice.Scissor && User1Choice == Round.OptionChoice.Rock)
            {
                return Round.Result.Win;
            }
            else if (User1Choice > User2Choice)
            {
                return Round.Result.Win;
            }

            return Round.Result.Lose;
        }
    }
}
