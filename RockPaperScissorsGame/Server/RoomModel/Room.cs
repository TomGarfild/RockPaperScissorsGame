using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Server.UserModel;

namespace Server.RoomModel
{
    public class Room
    {
        public string Id { get; set; }
        private List<User> users = new List<User>();
        public bool IsFull{get; set; }

        public Room(User user)
        {
            users.Add(user);
            IsFull = false;
            Id = Guid.NewGuid().ToString();
        }

        public bool AddUser(User user)
        {
            if (!IsFull)
            {
                users.Add(user);
                IsFull = true;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
