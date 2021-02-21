using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Server.UserModel;

namespace Server.RoomModel
{
    public class RoomCollect
    {
        private ConcurrentDictionary<string,Room> rooms = new ConcurrentDictionary<string, Room>();
        private static object ob = new object();
        public Room AddToRoom(User user)
        {
            lock (ob)
            {
                if (rooms.Any(r => !r.Value.IsFull))
                {
                    var room = rooms.First(r => !r.Value.IsFull).Value;
                    room.AddUser(user);
                    return room;
                }
                else
                {
                    var room = new Room(user);
                    rooms.TryAdd(room.Id,room);
                    return room;
                }
            }
        }

        public void Delete(string id)
        {
            lock (ob)
            {
                rooms.TryRemove(id, out _);
            }
        }
    }
}
