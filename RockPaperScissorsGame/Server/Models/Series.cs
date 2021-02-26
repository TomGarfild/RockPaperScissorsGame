using System;
using System.Collections.Generic;
using System.Threading;

namespace Server.Models
{
    public class Series
    {
        public Series()
        {
            
        }

        protected Round _round = new Round();
        private bool _checkResult = false;
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
            if (Users.Count == 1)
            {
                Users.Add(user);
                IsFull = true;
            }
            else
            {
                Users.Add(user);
            }
        }

        public bool IsRoundDone()
        {
            return _round.IsDone();
        }
        public virtual Round.Result GetResult(string user)
        {
            var res = _round.GetResult();
            if (Users[0] == user)
            {
                if (!_checkResult)
                {
                    _checkResult = true;
                }
                else
                {
                    _round.Clear();
                }
                return res;
            }
            else if (Users[1] == user)
            {
                if (!_checkResult)
                {
                    _checkResult = true;
                }
                else
                {
                   _round.Clear();
                }

                res = res switch
                {
                    Round.Result.Win => Round.Result.Lose,
                    Round.Result.Lose => Round.Result.Win,
                    _ => res
                };
                return res;
            }
            if (!_checkResult)
            {
                _checkResult = true;
            }
            else
            {
                _round.Clear();
            }
            return Round.Result.Undefine;
        }

        public void SetChoice1(string choice)
        {
            _round.SetChoice1(choice);
        }
        public void SetChoice2(string choice)
        {
            _round.SetChoice2(choice);
        }

        public CancellationToken ReturnToken()
        {
            return _round.Source.Token;
        }
    }
}
