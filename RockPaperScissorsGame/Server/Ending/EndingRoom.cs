using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Server.RoomModel;

namespace Server.Ending
{
    public class EndingRoom
    {
        private RoomCollect roomCollect;
        public EndingRoom( RoomCollect roomCollect)
        {
            this.roomCollect = roomCollect;
        }

        private readonly ConcurrentDictionary<string, (Task, CancellationTokenSource)> roomTasks 
            = new ConcurrentDictionary<string, (Task, CancellationTokenSource)>();

        public void SetEndingRoom(string id)
        {
            if (!roomTasks.ContainsKey(id))
            {
                var token = new CancellationTokenSource();
                var t = Delete(id, token.Token);
                roomTasks.TryAdd(id, (t,token));
            }
            else
            {
                roomTasks[id].Item2.Cancel();
                roomTasks.TryRemove(id, out _);
                var token = new CancellationTokenSource();
                var t = Delete(id, token.Token);
                roomTasks.TryAdd(id, (t, token));
            }

        }

        public async Task Delete(string id, CancellationToken token)
        {
           await Task.Delay(TimeSpan.FromMinutes(5),token);
           if(!token.IsCancellationRequested)
                roomCollect.Delete(id);
        }
    }
}
